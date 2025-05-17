let connection;

function connectToWebSocket() {
    console.log("Próba połączenia z SignalR...");

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    // Nasłuchiwanie nowych wiadomości
    connection.on("ReceiveMessage", (username, message) => {

        const messageContainer = document.querySelector(".message-container");

        const messageDiv = document.createElement("div");
        messageDiv.classList.add("message", username === "Nowak" ? "left" : "right");

        messageDiv.innerHTML = `
            <div style="font-size: 0.9em; color: #ccc;">
                <strong>${escapeHtml(username)}</strong>
                <span style="float: right;">${new Date().toLocaleString('pl-PL')}</span>
            </div>
            <div class="message-content">
                ${escapeHtml(message)}
                <div class="reaction-button-wrapper">
                    <span class="reaction-button" onclick="toggleReactions(this)">➕</span>
                    <div class="reaction-popup">
                        ${['❤️', '😂', '👍', '😮', '👎'].map(emoji => `
                            <button type="button"
                                class="emoji-button"
                                data-message-id="0"
                                data-emoji="${emoji}"
                                style="background:none; border:none; font-size:1.3em; cursor:pointer;">
                                ${emoji}
                            </button>
                        `).join('')}
                    </div>
                </div>
                <div class="reactions"></div>
            </div>
        `;

        messageContainer.appendChild(messageDiv);
        messageContainer.scrollTop = messageContainer.scrollHeight;
    });

    // Nasłuchiwanie reakcji
    connection.on("ReceiveEmoji", (messageId, emoji) => {
        console.log("Odebrano emoji:", emoji, "dla wiadomości ID:", messageId);
        // Zaktualizuj stronę, np. dodając emoji do odpowiedniej wiadomości
        updateReactions(messageId, emoji);
    });

    connection.start()
        .then(() => {
            console.log("Połączono z serwerem SignalR");
        })
        .catch(err => console.error("Błąd połączenia z SignalR: ", err));

    document.querySelector(".send-btn").addEventListener("click", function (e) {
        e.preventDefault();
        const textInput = document.querySelector('input[name="Text"]');
        const message = "ABCDEF"//textInput.value.trim();
        const username = "Janek"; // Możesz później zmienić na dynamiczne pobieranie

        if (message === "") return;

        connection.invoke("SendMessage", message)
            .then(() => {
                console.log("Wysłano wiadomość");
                textInput.value = "";
            })
            .catch(err => {
                console.error("Błąd wysyłania wiadomości: ", err);
            });
    });

    // Obsługa przycisku emoji (bez formularzy)
    document.addEventListener("click", function (e) {
        if (e.target.classList.contains("emoji-button")) {
            e.preventDefault();

            const messageId = parseInt(e.target.dataset.messageId);
            const emoji = e.target.dataset.emoji;

            connection.invoke("GiveEmoji", messageId, emoji)
                .then(() => console.log("Emoji wysłane:", emoji, "dla ID:", messageId))
                .catch(err => console.error("Błąd wysyłania emoji przez SignalR:", err));
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