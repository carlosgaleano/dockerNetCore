/**
 * @Author: Carlos Galeano
 * @Date:   2026-03-04 18:10:27
 * @Last Modified by:   Carlos Galeano
 * @Last Modified time: 2026-03-04 18:12:46
 */
-- Crea tabla de ejemplo en Postgres
CREATE TABLE IF NOT EXISTS todos (
  id SERIAL PRIMARY KEY,
  title VARCHAR(200) NOT NULL,
  is_done BOOLEAN NOT NULL DEFAULT FALSE,
  created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

INSERT INTO todos (title, is_done) VALUES ('Primer TODO en PostgreSQL', false);
