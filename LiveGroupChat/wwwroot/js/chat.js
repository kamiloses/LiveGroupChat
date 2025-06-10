console.log("DZIALA");
window.onload = function () {
    if (window.location.pathname === "/home") {
        connectToWebsockets();
    }
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

    connection.on("ReceiveMessage", (username, message, messageId, messageUserId) => {
        createMessageContainer(username, message, messageId, messageUserId);

    });

    connection.on("ReceiveEmoji", (messageId, emoji) => {
        console.log(emoji);
        updateReactions(messageId, emoji);
    });


    connection.start()
        .then(() => {
            console.log("Connected to SignalR server");

            document.querySelector(".send-btn").addEventListener("click", function (event) {
                event.preventDefault();
                let message = document.querySelector(".chat-input").value;
                connection.invoke("SendMessage", message)
            });

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


function createMessageContainer(username, message, messageId, messageUserId) {
    const myUserId = document.getElementById('user-id').getAttribute('data-user-id');
    const messageContainer = document.querySelector(".message-container");
    const messageDiv = document.createElement("div");

    if (myUserId == messageUserId) {
        messageDiv.classList.add("message-right");
    } else {
        messageDiv.classList.add("message-left");
    }

    messageDiv.setAttribute("data-message-id", messageId); 
    messageDiv.setAttribute("data-user-id", messageUserId);

    messageDiv.innerHTML = `
            <div style="font-size: 0.9em; color: #ccc;">
                <strong>${username}</strong>
            </div>
            <div class="message-content">
                 ${message}
                <div class="reaction-button-wrapper">
                    <span class="reaction-button" onclick="toggleReactions(this)">➕</span>
                    <div class="reaction-popup">
                        ${['❤️', '😂', '👍', '😮', '👎'].map(emoji => `
                            <button type="button"
                                class="emoji-button"
                                data-message-id="${messageId}"
                                data-emoji="${emoji}"
                                style="background:none; border:none; font-size:1.3em; cursor:pointer;">
                                ${emoji}
                            </button>`).join('')}
                    </div>
                </div>
                <div class="reactions"></div>
            </div>`;

    messageContainer.appendChild(messageDiv);
}

function updateReactions(messageId, emoji) {

    let messageDiv  = document.querySelector(`div[data-message-id="${messageId}"]`);

    if (!messageDiv) {
        console.warn(`Message with ID ${messageId} not found. Waiting for it to be created...`);
        return;
    }

    const reactionsDiv = messageDiv.querySelector(".reactions");
    const emojiSpan = document.createElement("span");
    emojiSpan.textContent = emoji;
    emojiSpan.style.fontSize = "1.3em";
    emojiSpan.style.marginRight = "5px";
    reactionsDiv.appendChild(emojiSpan);
}
