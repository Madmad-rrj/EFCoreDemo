const { Pool } = require("pg");

const pool = new Pool({
    host: process.env.PGHOST || "localhost",
    port: Number(process.env.PGPORT) || 5433,
    database: process.env.PGDATABASE || "efcore_demo",
    user: process.env.PGUSER || "postgres",
    password: process.env.PGPASSWORD
});

module.exports = pool;