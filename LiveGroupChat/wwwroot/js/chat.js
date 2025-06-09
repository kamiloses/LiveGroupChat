console.log("DZIALA");
window.onload = function () {
  if (window.location.pathname === "/home") {  
    connectToWebsockets();}
}

function connectToWebsockets() {
    console.log("Trying to connect to SignalR...");
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/ws")
        .build();


}

