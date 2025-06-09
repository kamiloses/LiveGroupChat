console.log("DZIALA");
window.onload = function () {
  if (window.location.pathname === "/home") {  
    connectToWebsockets();}
}

function toggleReactions(el) {
    const popup = el.nextElementSibling;
    document.querySelectorAll('.reaction-popup').forEach(p => {
        if (p !== popup) p.style.display = 'none';
    });
    popup.style.display = popup.style.display === "block" ? "none" : "block";
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

    connection.on("ReceiveEmoji", (messageId,emoji) => {
        console.log(emoji);
    });
    

    connection.start()
        .then(() => {
            console.log("Connected to SignalR server");

            document.querySelector(".send-btn").addEventListener("click", function (event) {
                event.preventDefault();
                let message = document.querySelector(".chat-input").value;
                 connection.invoke("SendMessage", message)});
            
            //emotki
            document.addEventListener("click", function (e) {
                if (e.target.classList.contains("emoji-button")) {
                    e.preventDefault();

                    const messageId = parseInt(e.target.dataset.messageId);
                    const emoji = e.target.dataset.emoji;
                    connection.invoke("GiveEmoji", messageId, emoji)
                }
                
            })
            
            
            
            
            
        })
        .catch(err => console.error("SignalR connection error: ", err));
}

