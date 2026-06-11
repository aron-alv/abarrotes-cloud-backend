-- 1. Crear la nueva tabla de suscripciones
CREATE TABLE suscripciones (
  id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  tiendaid uuid REFERENCES tiendas(id) ON DELETE CASCADE,
  monto numeric NOT NULL,
  fechapago timestamp with time zone DEFAULT now(),
  periodoinicio timestamp with time zone NOT NULL,
  metodopago varchar NOT NULL,
  estatus varchar NOT NULL
);

-- 2. Agregar los campos nuevos a la tabla tiendas (que ya existe)
ALTER TABLE tiendas 
ADD COLUMN estatuslicencia varchar DEFAULT 'activo',
ADD COLUMN fechavencimiento timestamp with time zone;