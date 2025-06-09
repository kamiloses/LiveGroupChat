console.log("DZIALA");
window.onload = function () {
  if (window.location.pathname === "/home") {  
    connectToWebsockets();}
}



let connection;

function connectToWebsockets() {
    console.log("Trying to connect to SignalR...");
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/ws")
        .build();

    connection.on("ReceiveMessage", (username, message, messageId) => {
        console.log(message);
    });

    connection.start()
        .then(() => {
            console.log("Connected to SignalR server");

            document.querySelector(".send-btn").addEventListener("click", function (event) {
                event.preventDefault();
                let message = document.querySelector(".chat-input").value;
                connection.invoke("SendMessage", message)
                    .then(() => {
                        console.log("SENT MESSAGE");
                    })
                    .catch(err => {
                        console.error("Error sending message: ", err);
                    });
            });

        })
        .catch(err => console.error("SignalR connection error: ", err));
}