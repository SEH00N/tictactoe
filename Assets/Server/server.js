const ws = require('ws');
const server = new ws.Server({port:3031});

const room = require("./room.js");
const messageSender = require("./messageSender.js");

server.on('listening', () => {
    console.log("Server Opened on Port 3031");
});

server.on('connection', client => {
    console.log("Client Connected");

    client.on('message', message => {
        const packet = JSON.parse(message);
        console.log(packet);

        switch(packet.l)
        {
            case 'room':
                roomData(client, packet);
                break;
            case 'game':
                gameData(client, packet);
                break;
        }
    });    
});

const roomData = (client, packet) => {
    switch(packet.t)
    {
        case 'createReq':
            room.create(client);
            break;
        case 'joinReq':
            room.join(packet, client);
            break;
    }
}

const gameData = (client, packet) => {
    switch(packet.t)
    {
        case 'setted':
            messageSender.sendMessages(room.room[client.room], client, JSON.stringify(packet));
            break;
    }
}