window.onload = function () {
    connectToWebSocket();
};

function toggleReactions(el) {
    const popup = el.nextElementSibling;
    document.querySelectorAll('.reaction-popup').forEach(p => {
        if (p !== popup) p.style.display = 'none';
    });
    popup.style.display = popup.style.display === "block" ? "none" : "block";
}

document.addEventListener('click', function (e) {
    if (!e.target.closest('.reaction-button-wrapper')) {
        document.querySelectorAll('.reaction-popup').forEach(p => p.style.display = 'none');
    }
});

let connection;

function connectToWebSocket() {
    console.log("Trying to connect to SignalR...");

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    connection.on("ReceiveMessage", (username, message) => {
        const messageContainer = document.querySelector(".message-container");

        const messageDiv = document.createElement("div");
        messageDiv.classList.add("message", username === "Nowak" ? "left" : "right");

        messageDiv.innerHTML =
            `<div style="font-size: 0.9em; color: #ccc;">
                <strong>${escapeHtml(username)}</strong>
                <span style="float: right;">${new Date().toLocaleString('en-US')}</span>
            </div>
            <div class="message-content">
                ${escapeHtml(message)}
                <div class="reaction-button-wrapper">
                    <span class="reaction-button" onclick="toggleReactions(this)">➕</span>
                    <div class="reaction-popup">
                        ${['❤️', '😂', '👍', '😮', '👎'].map(emoji =>
                `<button type="button"
                                class="emoji-button"
                                data-message-id="0"
                                data-emoji="${emoji}"
                                style="background:none; border:none; font-size:1.3em; cursor:pointer;">
                                ${emoji}
                            </button>`
            ).join('')}
                    </div>
                </div>
                <div class="reactions"></div>
            </div>`;

        messageContainer.appendChild(messageDiv);
        messageContainer.scrollTop = messageContainer.scrollHeight;
    });

    connection.on("ReceiveEmoji", (messageId, emoji) => {
        updateReactions(messageId, emoji);
    });

    connection.start()
        .then(() => {
            console.log("Connected to SignalR server");
        })
        .catch(err => console.error("SignalR connection error: ", err));

    document.querySelector(".send-btn").addEventListener("click", function (e) {
        e.preventDefault();
        const textInput = document.querySelector('input[name="Text"]');
        const message = "ABCDEF";
        const username = "Janek";

        if (message === "") return;

        connection.invoke("SendMessage", message)
            .then(() => {
                textInput.value = "";
            })
            .catch(err => {
                console.error("Error sending message: ", err);
            });
    });

    document.addEventListener("click", function (e) {
        if (e.target.classList.contains("emoji-button")) {
            e.preventDefault();

            const messageId = parseInt(e.target.dataset.messageId);
            const emoji = e.target.dataset.emoji;

            connection.invoke("GiveEmoji", messageId, emoji)
                .then(() => console.log("Emoji sent:", emoji, "for ID:", messageId))
                .catch(err => console.error("Error sending emoji via SignalR:", err));
        }
    });
}

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function updateReactions(messageId, emoji) {
    const messageDiv = document.querySelector(`.message[data-message-id="${messageId}"]`);
    if (messageDiv) {
        const reactionsDiv = messageDiv.querySelector(".reactions");
        const emojiSpan = document.createElement("span");
        emojiSpan.textContent = emoji;
        reactionsDiv.appendChild(emojiSpan);
    }
}
