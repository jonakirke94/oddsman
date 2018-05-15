const express = require("express");
const router = express.Router();
const msg = require("../db/http");
const moment = require('moment');
const seq = require('../models');
const Tournament = seq.tournaments;
const Request = seq.requests;
const Bet = seq.bets;
const Match = seq.matches;
const Tournament_User = seq.users_tournaments;
const helper = require('../controllers/helper');
const User = seq.users;
const {
    Op
} = require('sequelize')
const scraper = require('../services/scraper');


let today = moment();

exports.edit_match = (req, res, next) => {
    const tourId = req.params.tourid;
    const matchId = req.params.matchid;
    // Convert match values to all lowercase
    const match = keysToLower(req.body);

    Match.findById(matchId)
        .then((m) => {
            if (!m) return msg.show404(req, res, "The match could not be found");
            // foreach key in the database match
            Object.keys(m.dataValues).forEach((key) => {
                // Convert the key to lowercase and use it to access the new match's values
                // After that set the database match's original keys to the new values
                try {
                    const k = key.toLowerCase();
                    const val = match[k];
                    if (val) {
                        m[key] = val;
                    }
                } catch (error) {
                    console.log("failed patching match: " + error);
                }
            });

            m.save().then(() => {
                return msg.show200(req, res, "Success", m);
            });
        })
        .catch(err => {
            return msg.show500(req, res, err);
        })
}


/* HELPERS */

function keysToLower(obj) {
    let key, keys = Object.keys(obj);
    let n = keys.length;
    let newObj = {}
    while (n--) {
        key = keys[n];
        newObj[key.toLowerCase()] = obj[key];
    }
    return newObj;
}