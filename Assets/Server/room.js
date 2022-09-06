const messageSender = require("./messageSender.js");

let room = {}

const create = function (client) {
    try {
        client.room = randCode();
        if(room[client.room] == undefined)
            room[client.room] = [];

        room[client.room].push(client);
        console.log(client.room + " : Room Created");
        client.send(JSON.stringify({
            l : "room",
            t : "createRes",
            v : client.room
        }));
    }
    catch (error) {
        client.send(JSON.stringify({
            l : "error",
            t : "error",
            v : error
        }));
    }
}

const join = function (packet, client) {
    try {
        client.room = packet.v;
        console.log(client.room);
        room[client.room].push(client);
    
        console.log(client.room + " : Room Join");
        client.send(JSON.stringify({
            l: "room",
            t: "joinRes",
            v: ""
        }));
        messageSender.sendMessages(room[client.room], client, JSON.stringify({
            l : "room",
            t : "joined",
            v : ""
        }));
    }
    catch (e) {
        console.log(e.message);
        client.send(JSON.stringify({
            l : "error",
            t : "error",
            v : e.message
        }));
    }
}

const quit = function (client) {
    try {
        console.log(client.room + " : Room Quit");

        client.send(JSON.stringify({
            l : "room",
            t : "quitRes",
            v : ""
        }));
        messageSender.sendMessages(room[client.room], client, JSON.stringify({
            l : "room",
            t : "quitted",
            v : client.order
        }));

        room[client.room][client.order] = undefined;
        client.room = undefined;
        client.order = undefined;
    }
    catch {
        client.send(JSON.stringify({
            l : "error",
            t : "error",
            v : "Failed to Exit"
        }));
    }
}

const randCode = () => {
    const code = Math.random().toString(36).substring(2, 11).toUpperCase();
    return code;
}

exports.create = create;
exports.join = join;
exports.quit = quit;
exports.room = room;