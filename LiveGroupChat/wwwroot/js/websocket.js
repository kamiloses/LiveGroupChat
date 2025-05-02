function connectToWebSocket() {
    console.log("Próba połączenia z SignalR...");
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    // Nasłuchuj na wiadomości z serwera
    connection.on("ReceiveMessage", (username, message) => {
        console.log(`${username}: ${message}`);
        // Możesz tu dodać logikę do wyświetlania wiadomości na stronie
    });

    connection.start()
        .then(() => {
            console.log("Połączono z serwerem SignalR");
        })
        .catch(err => console.error("Błąd połączenia z SignalR: ", err));

    // Możesz dodać funkcjonalność wysyłania wiadomości z klienta
    document.querySelector(".send-btn").addEventListener("click", function(e) {
        e.preventDefault();  // Zapobiegaj domyślnemu wysyłaniu formularza
        const text = document.querySelector('input[name="Text"]').value;
        const username = "Janek";  // Możesz pobrać nazwisko użytkownika z sesji lub innego źródła

        connection.invoke("SendMessage", username, text)
            .then(() => {
                console.log("Wysłano wiadomość");
            })
            .catch(err => {
                console.error("Błąd wysyłania wiadomości: ", err);
            });
    });
}
