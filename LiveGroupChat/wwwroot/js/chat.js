const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", (user, message) => {
    const msgContainer = document.querySelector(".message-container");

    const messageDiv = document.createElement("div");
    messageDiv.classList.add("message", "left");

    const userNameDiv = document.createElement("div");
    userNameDiv.style.fontSize = "0.9em";
    userNameDiv.style.color = "#ccc";
    userNameDiv.innerHTML = `<strong>${user}</strong>`;

    const messageContent = document.createElement("div");
    messageContent.classList.add("message-content");
    messageContent.textContent = message;

    const reactionWrapper = document.createElement("div");
    reactionWrapper.classList.add("reaction-button-wrapper");

    const reactionButton = document.createElement("span");
    reactionButton.classList.add("reaction-button");
    reactionButton.textContent = "➕";
    reactionButton.onclick = function () {
        toggleReactions(reactionButton);
    };

    const reactionPopup = document.createElement("div");
    reactionPopup.classList.add("reaction-popup");

    const emojis = ["❤️", "😂", "👍", "😮", "👎"];

    emojis.forEach(e => {
        const btn = document.createElement("button");
        btn.type = "button";
        btn.classList.add("emoji-button");
        btn.textContent = e;
        btn.onclick = () => {
            reactionPopup.style.display = "none";
        };
        reactionPopup.appendChild(btn);
    });

    reactionWrapper.appendChild(reactionButton);
    reactionWrapper.appendChild(reactionPopup);

    messageContent.appendChild(reactionWrapper);

    const reactionsDiv = document.createElement("div");
    reactionsDiv.classList.add("reactions");

    messageContent.appendChild(reactionsDiv);

    messageDiv.appendChild(userNameDiv);
    messageDiv.appendChild(messageContent);

    msgContainer.appendChild(messageDiv);
    msgContainer.scrollTop = msgContainer.scrollHeight;
});

connection.start().catch(err => console.error(err.toString()));

function toggleReactions(button) {
    const popup = button.nextElementSibling;
    if (popup.style.display === "block") {
        popup.style.display = "none";
    } else {
        popup.style.display = "block";
    }
}

