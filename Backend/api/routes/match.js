const express = require("express");
const router = express.Router();
const guard = require('../guards/authguard');
const adminguard = require('../guards/adminguard');

const matchController = require('../../controllers/match');


router.get("/missing", adminguard, matchController.get_missing_matches);
router.get("/result/missing", adminguard, matchController.get_missing_results);
router.post('/results', matchController.get_results);

router.patch("/:matchid", adminguard, matchController.edit_match);
router.patch("/result/:resultid", adminguard, matchController.edit_result);


module.exports = router;