const express = require("express");
const pool = require("./db");

const app = express();

app.use(express.json());

app.get("/api/test-db", async (req, res) => {
    try {
        const result = await pool.query("SELECT NOW()");

        res.json({
            message: "Kết nối PostgreSQL thành công",
            time: result.rows[0].now
        });
    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Không thể kết nối PostgreSQL"
        });
    }
});

app.get("/api/products", async (req, res) => {
    try {
        const result = await pool.query('SELECT * FROM "Products"');

        res.status(200).json(result.rows);
    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Không thể lấy danh sách sản phẩm"
        });
    }
});

app.post("/api/products", async (req, res) => {
    try {
        const { name, price, quantity } = req.body;

        const result = await pool.query(
            `INSERT INTO "Products" ("Name", "Price", "Quantity")
             VALUES ($1, $2, $3)
             RETURNING *`,
            [name, price, quantity]
        );

        res.status(201).json(result.rows[0]);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Không thể thêm sản phẩm"
        });
    }
});

app.listen(3000, () => {
    console.log("API Server chạy tại http://localhost:3000");
});

