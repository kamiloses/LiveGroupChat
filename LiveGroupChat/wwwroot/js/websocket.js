let stompClient = null
let chatId = null;
let currentSubscription = null;
let isPublicChat="false";

function connectToWebSocket() {


    const socket = new SockJS('http://localhost:8080/ws');
    stompClient = Stomp.over(socket);

    stompClient.connect({}, function (frame) {

        console.log('Connected: ' + frame);
        
       stompClient.subscribe('/topic/public/group', function (message) {
           var activeUsers = JSON.parse(message.body);


                
            })}