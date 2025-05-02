// using System.Net.WebSockets;
// using System.Text;
//
// namespace LiveGroupChat.Middlewares;
//
// public class WebSocketMiddleware
// {
//     private readonly RequestDelegate _next;
//
//     public WebSocketMiddleware(RequestDelegate next)
//     {
//         _next = next;
//     }
//
//     public async Task InvokeAsync(HttpContext context)
//     {
//         if (context.Request.Path == "/ws")
//         {
//             if (context.WebSockets.IsWebSocketRequest)
//             {
//                 using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
//                 Console.WriteLine("Connected to ws");
//
//                 var buffer = new byte[1024 * 4];
//
//                 while (webSocket.State == WebSocketState.Open)
//                 {
//                     var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
//
//                     if (result.MessageType == WebSocketMessageType.Text)
//                     {
//                         var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
//                         Console.WriteLine($"Odebrano wiadomość: {message}");
//
//                         var echoMessage = Encoding.UTF8.GetBytes("Echo: " + message);
//                         await webSocket.SendAsync(new ArraySegment<byte>(echoMessage), WebSocketMessageType.Text, true, CancellationToken.None);
//                     }
//                     else if (result.MessageType == WebSocketMessageType.Close)
//                     {
//                         await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Zamykam", CancellationToken.None);
//                     }
//                 }
//             }
//             else
//             {
//                 context.Response.StatusCode = 400;
//             }
//         }
//         else
//         {
//             await _next(context);
//         }
//     }
// }