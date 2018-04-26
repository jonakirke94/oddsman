const express = require("express");
const router = express.Router();
const db = require(".././../db/db");
const msg = require(".././../db/http");
const guard = require('../guards/authguard');

const userController = require('../../controllers/user');

router.get("/signup", userController.user_signup);
router.get("/all", guard, userController.get_all);
router.post("/", userController.user_signup);

module.exports = router;
