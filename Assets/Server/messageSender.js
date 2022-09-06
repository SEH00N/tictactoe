const sendMessages = function (room, performer, packet, isGlobal = false) {
    if(room == undefined) return;

    if(!isGlobal) {
        room.forEach(client => {
            if(client != undefined && client != performer)
                client.send(packet);
        })
    }
    else {
        room.forEach(client => {
            if(client != undefined)
                client.send(packet);
        });
    }
}

exports.sendMessages = sendMessages;