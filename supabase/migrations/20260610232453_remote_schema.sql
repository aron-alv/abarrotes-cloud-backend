


SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;


COMMENT ON SCHEMA "public" IS 'standard public schema';



CREATE EXTENSION IF NOT EXISTS "pg_stat_statements" WITH SCHEMA "extensions";






CREATE EXTENSION IF NOT EXISTS "pgcrypto" WITH SCHEMA "extensions";






CREATE EXTENSION IF NOT EXISTS "supabase_vault" WITH SCHEMA "vault";






CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA "extensions";





SET default_tablespace = '';

SET default_table_access_method = "heap";


CREATE TABLE IF NOT EXISTS "public"."abonoscredito" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "clienteid" "uuid" NOT NULL,
    "tiendaid" "uuid" NOT NULL,
    "usuarioid" "uuid" NOT NULL,
    "montoabonado" numeric(10,2) NOT NULL,
    "fechaabono" timestamp with time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE "public"."abonoscredito" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."categorias" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "tiendaid" "uuid" NOT NULL,
    "nombre" character varying(150) NOT NULL
);


ALTER TABLE "public"."categorias" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."clientescredito" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "tiendaid" "uuid" NOT NULL,
    "nombrecompleto" character varying(255) NOT NULL,
    "telefono" character varying(20),
    "limitecredito" numeric(10,2) DEFAULT 0.00 NOT NULL,
    "saldoactual" numeric(10,2) DEFAULT 0.00 NOT NULL,
    "estatus" character varying(50) DEFAULT 'Activo'::character varying,
    CONSTRAINT "clientescredito_estatus_check" CHECK ((("estatus")::"text" = ANY ((ARRAY['Activo'::character varying, 'Suspendido'::character varying])::"text"[])))
);


ALTER TABLE "public"."clientescredito" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."detalleventas" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "ventaid" "uuid" NOT NULL,
    "productoid" "uuid" NOT NULL,
    "cantidad" numeric(10,3) NOT NULL,
    "preciounitario" numeric(10,2) NOT NULL,
    "subtotal" numeric(10,2) NOT NULL
);


ALTER TABLE "public"."detalleventas" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."kardex" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "tiendaid" "uuid" NOT NULL,
    "productoid" "uuid" NOT NULL,
    "usuarioid" "uuid" NOT NULL,
    "tipomovimiento" character varying(50) NOT NULL,
    "motivo" character varying(100) NOT NULL,
    "cantidad" numeric(10,3) NOT NULL,
    "fechamovimiento" timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "kardex_motivo_check" CHECK ((("motivo")::"text" = ANY ((ARRAY['Venta'::character varying, 'Compra Proveedor'::character varying, 'Merma'::character varying, 'Ajuste Manual'::character varying])::"text"[]))),
    CONSTRAINT "kardex_tipomovimiento_check" CHECK ((("tipomovimiento")::"text" = ANY ((ARRAY['Entrada'::character varying, 'Salida'::character varying])::"text"[])))
);


ALTER TABLE "public"."kardex" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."listablanca" (
    "id" "uuid" DEFAULT "extensions"."uuid_generate_v4"() NOT NULL,
    "email" "text" NOT NULL,
    "tienda_id" "uuid" NOT NULL,
    "fecha_agregado" timestamp with time zone DEFAULT "now"()
);


ALTER TABLE "public"."listablanca" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."productos" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "tiendaid" "uuid" NOT NULL,
    "categoriaid" "uuid" NOT NULL,
    "codigobarras" character varying(100),
    "nombre" character varying(255) NOT NULL,
    "preciocompra" numeric(10,2) DEFAULT 0.00 NOT NULL,
    "precioventa" numeric(10,2) DEFAULT 0.00 NOT NULL,
    "tipounidad" character varying(50) NOT NULL,
    "stockactual" numeric(10,3) DEFAULT 0.000 NOT NULL,
    "stockminimo" numeric(10,3) DEFAULT 0.000 NOT NULL,
    CONSTRAINT "chk_stock_piezas" CHECK (((("tipounidad")::"text" <> 'Pieza'::"text") OR ((("stockactual" % (1)::numeric) = (0)::numeric) AND (("stockminimo" % (1)::numeric) = (0)::numeric)))),
    CONSTRAINT "productos_tipounidad_check" CHECK ((("tipounidad")::"text" = ANY ((ARRAY['Pieza'::character varying, 'Kg'::character varying, 'Litro'::character varying])::"text"[])))
);


ALTER TABLE "public"."productos" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."tiendas" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "nombrenegocio" character varying(255) NOT NULL,
    "fecharegistro" timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    "estatus" character varying(50) DEFAULT 'Activo'::character varying
);


ALTER TABLE "public"."tiendas" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."usuarios" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "tiendaid" "uuid" NOT NULL,
    "nombrecompleto" character varying(255) NOT NULL,
    "rol" character varying(50) NOT NULL,
    "pinventa" character varying(4),
    "email" character varying(255),
    CONSTRAINT "usuarios_rol_check" CHECK ((("rol")::"text" = ANY ((ARRAY['Admin'::character varying, 'Cajero'::character varying])::"text"[])))
);


ALTER TABLE "public"."usuarios" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."ventas" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "tiendaid" "uuid" NOT NULL,
    "usuarioid" "uuid" NOT NULL,
    "clientecreditoid" "uuid",
    "fechaventa" timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    "total" numeric(10,2) NOT NULL,
    "metodopago" character varying(50) DEFAULT 'Efectivo'::character varying NOT NULL,
    "estatus" character varying(50) DEFAULT 'Completada'::character varying,
    CONSTRAINT "ventas_estatus_check" CHECK ((("estatus")::"text" = ANY ((ARRAY['Completada'::character varying, 'Cancelada'::character varying])::"text"[]))),
    CONSTRAINT "ventas_metodopago_check" CHECK ((("metodopago")::"text" = ANY ((ARRAY['Efectivo'::character varying, 'Tarjeta'::character varying, 'Transferencia'::character varying, 'Crédito'::character varying])::"text"[])))
);


ALTER TABLE "public"."ventas" OWNER TO "postgres";


ALTER TABLE ONLY "public"."abonoscredito"
    ADD CONSTRAINT "abonoscredito_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."categorias"
    ADD CONSTRAINT "categorias_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."clientescredito"
    ADD CONSTRAINT "clientescredito_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."detalleventas"
    ADD CONSTRAINT "detalleventas_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."kardex"
    ADD CONSTRAINT "kardex_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."listablanca"
    ADD CONSTRAINT "listablanca_email_key" UNIQUE ("email");



ALTER TABLE ONLY "public"."listablanca"
    ADD CONSTRAINT "listablanca_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."productos"
    ADD CONSTRAINT "productos_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."tiendas"
    ADD CONSTRAINT "tiendas_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."usuarios"
    ADD CONSTRAINT "usuarios_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."ventas"
    ADD CONSTRAINT "ventas_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."abonoscredito"
    ADD CONSTRAINT "abonoscredito_clienteid_fkey" FOREIGN KEY ("clienteid") REFERENCES "public"."clientescredito"("id");



ALTER TABLE ONLY "public"."abonoscredito"
    ADD CONSTRAINT "abonoscredito_tiendaid_fkey" FOREIGN KEY ("tiendaid") REFERENCES "public"."tiendas"("id");



ALTER TABLE ONLY "public"."abonoscredito"
    ADD CONSTRAINT "abonoscredito_usuarioid_fkey" FOREIGN KEY ("usuarioid") REFERENCES "public"."usuarios"("id");



ALTER TABLE ONLY "public"."categorias"
    ADD CONSTRAINT "categorias_tiendaid_fkey" FOREIGN KEY ("tiendaid") REFERENCES "public"."tiendas"("id");



ALTER TABLE ONLY "public"."clientescredito"
    ADD CONSTRAINT "clientescredito_tiendaid_fkey" FOREIGN KEY ("tiendaid") REFERENCES "public"."tiendas"("id");



ALTER TABLE ONLY "public"."detalleventas"
    ADD CONSTRAINT "detalleventas_productoid_fkey" FOREIGN KEY ("productoid") REFERENCES "public"."productos"("id");



ALTER TABLE ONLY "public"."detalleventas"
    ADD CONSTRAINT "detalleventas_ventaid_fkey" FOREIGN KEY ("ventaid") REFERENCES "public"."ventas"("id");



ALTER TABLE ONLY "public"."kardex"
    ADD CONSTRAINT "kardex_productoid_fkey" FOREIGN KEY ("productoid") REFERENCES "public"."productos"("id");



ALTER TABLE ONLY "public"."kardex"
    ADD CONSTRAINT "kardex_tiendaid_fkey" FOREIGN KEY ("tiendaid") REFERENCES "public"."tiendas"("id");



ALTER TABLE ONLY "public"."kardex"
    ADD CONSTRAINT "kardex_usuarioid_fkey" FOREIGN KEY ("usuarioid") REFERENCES "public"."usuarios"("id");



ALTER TABLE ONLY "public"."productos"
    ADD CONSTRAINT "productos_categoriaid_fkey" FOREIGN KEY ("categoriaid") REFERENCES "public"."categorias"("id");



ALTER TABLE ONLY "public"."productos"
    ADD CONSTRAINT "productos_tiendaid_fkey" FOREIGN KEY ("tiendaid") REFERENCES "public"."tiendas"("id");



ALTER TABLE ONLY "public"."usuarios"
    ADD CONSTRAINT "usuarios_tiendaid_fkey" FOREIGN KEY ("tiendaid") REFERENCES "public"."tiendas"("id");



ALTER TABLE ONLY "public"."ventas"
    ADD CONSTRAINT "ventas_clientecreditoid_fkey" FOREIGN KEY ("clientecreditoid") REFERENCES "public"."clientescredito"("id");



ALTER TABLE ONLY "public"."ventas"
    ADD CONSTRAINT "ventas_tiendaid_fkey" FOREIGN KEY ("tiendaid") REFERENCES "public"."tiendas"("id");



ALTER TABLE ONLY "public"."ventas"
    ADD CONSTRAINT "ventas_usuarioid_fkey" FOREIGN KEY ("usuarioid") REFERENCES "public"."usuarios"("id");



ALTER TABLE "public"."abonoscredito" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."categorias" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."clientescredito" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."detalleventas" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."kardex" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."productos" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."tiendas" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."ventas" ENABLE ROW LEVEL SECURITY;




ALTER PUBLICATION "supabase_realtime" OWNER TO "postgres";


GRANT USAGE ON SCHEMA "public" TO "postgres";
GRANT USAGE ON SCHEMA "public" TO "anon";
GRANT USAGE ON SCHEMA "public" TO "authenticated";
GRANT USAGE ON SCHEMA "public" TO "service_role";





































































































































































GRANT ALL ON TABLE "public"."abonoscredito" TO "anon";
GRANT ALL ON TABLE "public"."abonoscredito" TO "authenticated";
GRANT ALL ON TABLE "public"."abonoscredito" TO "service_role";



GRANT ALL ON TABLE "public"."categorias" TO "anon";
GRANT ALL ON TABLE "public"."categorias" TO "authenticated";
GRANT ALL ON TABLE "public"."categorias" TO "service_role";



GRANT ALL ON TABLE "public"."clientescredito" TO "anon";
GRANT ALL ON TABLE "public"."clientescredito" TO "authenticated";
GRANT ALL ON TABLE "public"."clientescredito" TO "service_role";



GRANT ALL ON TABLE "public"."detalleventas" TO "anon";
GRANT ALL ON TABLE "public"."detalleventas" TO "authenticated";
GRANT ALL ON TABLE "public"."detalleventas" TO "service_role";



GRANT ALL ON TABLE "public"."kardex" TO "anon";
GRANT ALL ON TABLE "public"."kardex" TO "authenticated";
GRANT ALL ON TABLE "public"."kardex" TO "service_role";



GRANT ALL ON TABLE "public"."listablanca" TO "anon";
GRANT ALL ON TABLE "public"."listablanca" TO "authenticated";
GRANT ALL ON TABLE "public"."listablanca" TO "service_role";



GRANT ALL ON TABLE "public"."productos" TO "anon";
GRANT ALL ON TABLE "public"."productos" TO "authenticated";
GRANT ALL ON TABLE "public"."productos" TO "service_role";



GRANT ALL ON TABLE "public"."tiendas" TO "anon";
GRANT ALL ON TABLE "public"."tiendas" TO "authenticated";
GRANT ALL ON TABLE "public"."tiendas" TO "service_role";



GRANT ALL ON TABLE "public"."usuarios" TO "anon";
GRANT ALL ON TABLE "public"."usuarios" TO "authenticated";
GRANT ALL ON TABLE "public"."usuarios" TO "service_role";



GRANT ALL ON TABLE "public"."ventas" TO "anon";
GRANT ALL ON TABLE "public"."ventas" TO "authenticated";
GRANT ALL ON TABLE "public"."ventas" TO "service_role";









ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES TO "service_role";






ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS TO "service_role";






ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES TO "service_role";































drop extension if exists "pg_net";

alter table "public"."clientescredito" drop constraint "clientescredito_estatus_check";

alter table "public"."kardex" drop constraint "kardex_motivo_check";

alter table "public"."kardex" drop constraint "kardex_tipomovimiento_check";

alter table "public"."productos" drop constraint "productos_tipounidad_check";

alter table "public"."usuarios" drop constraint "usuarios_rol_check";

alter table "public"."ventas" drop constraint "ventas_estatus_check";

alter table "public"."ventas" drop constraint "ventas_metodopago_check";

alter table "public"."clientescredito" add constraint "clientescredito_estatus_check" CHECK (((estatus)::text = ANY ((ARRAY['Activo'::character varying, 'Suspendido'::character varying])::text[]))) not valid;

alter table "public"."clientescredito" validate constraint "clientescredito_estatus_check";

alter table "public"."kardex" add constraint "kardex_motivo_check" CHECK (((motivo)::text = ANY ((ARRAY['Venta'::character varying, 'Compra Proveedor'::character varying, 'Merma'::character varying, 'Ajuste Manual'::character varying])::text[]))) not valid;

alter table "public"."kardex" validate constraint "kardex_motivo_check";

alter table "public"."kardex" add constraint "kardex_tipomovimiento_check" CHECK (((tipomovimiento)::text = ANY ((ARRAY['Entrada'::character varying, 'Salida'::character varying])::text[]))) not valid;

alter table "public"."kardex" validate constraint "kardex_tipomovimiento_check";

alter table "public"."productos" add constraint "productos_tipounidad_check" CHECK (((tipounidad)::text = ANY ((ARRAY['Pieza'::character varying, 'Kg'::character varying, 'Litro'::character varying])::text[]))) not valid;

alter table "public"."productos" validate constraint "productos_tipounidad_check";

alter table "public"."usuarios" add constraint "usuarios_rol_check" CHECK (((rol)::text = ANY ((ARRAY['Admin'::character varying, 'Cajero'::character varying])::text[]))) not valid;

alter table "public"."usuarios" validate constraint "usuarios_rol_check";

alter table "public"."ventas" add constraint "ventas_estatus_check" CHECK (((estatus)::text = ANY ((ARRAY['Completada'::character varying, 'Cancelada'::character varying])::text[]))) not valid;

alter table "public"."ventas" validate constraint "ventas_estatus_check";

alter table "public"."ventas" add constraint "ventas_metodopago_check" CHECK (((metodopago)::text = ANY ((ARRAY['Efectivo'::character varying, 'Tarjeta'::character varying, 'Transferencia'::character varying, 'Crédito'::character varying])::text[]))) not valid;

alter table "public"."ventas" validate constraint "ventas_metodopago_check";


