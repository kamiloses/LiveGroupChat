function connectToWebSocket() {
    const socket = new WebSocket("ws://" + window.location.host + "/ws");

    socket.onopen = () => {
        console.log("ŁACZEE SIEE");
        socket.send("Witaj serwerze!");
    };

}













//
// socket.onmessage = (event) => {
//     const msg = document.createElement("div");
//     msg.textContent = "Odebrano: " + event.data;
//     document.getElementById("messages").appendChild(msg);
// };
//
// socket.onclose = () => {
//     console.log("Połączenie zamknięte");
// };
//
// socket.onerror = (err) => {
//     console.error("WebSocket error:", err);
// };