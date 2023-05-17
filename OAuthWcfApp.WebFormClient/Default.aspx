<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OAuthWcfApp.WebFormClient.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OAuth 2.0 Demo</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#login').click(function (event) {
                event.preventDefault();
                var login = $('#login-input').val();
                var password = $('#password-input').val();
                var requestData = {
                    Login: login,
                    Password: password
                };

                $.ajax({
                    type: "POST",
                    url: "http://localhost:8089/oAuthService/Services/TokenService.svc/Authorize",
                    data: JSON.stringify(requestData),
                    contentType: "application/json",
                    dataType: "text",
                    success: function (data, status, xhr) {
                        $('#accessToken').text(data);
                        $('#login-error').text("");
                        $('#login-error').text("Login approved: " + data);
                        $('#get-user').removeAttr('disabled');
                    },
                    error: function (xhr, status, error) {
                        $('#accessToken').text("");
                        $('#login-error').text("Login failed: " + xhr.responseText);
                        $('#get-user').attr('disabled', 'disabled');
                    }
                });
            });

            $('#get-user').click(function (event) {
                event.preventDefault();
                var accessToken = $('#accessToken').text();

                $.ajax({
                    type: "GET",
                    url: "http://localhost:8089/oAuthService/Services/UserService.svc/GetUser",
                    headers: {
                        "Authorization": "Bearer " + accessToken
                    },
                    dataType: "text",
                    success: function (data) {
                        $('#user').text(data);
                    },
                    error: function (xhr, status, error) {
                        $('#user').text("Error: " + xhr.responseText);
                    }
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="login-input">Login:</label>
            <input type="text" id="login-input" />
            <br />
            <label for="password-input">Password:</label>
            <input type="password" id="password-input" />
            <br />
            <button id="login">Get Auth Grant</button>
            <div id="accessToken"></div>
            <div id="login-error" style="color: red;"></div>
        </div>

        <br />

        <button id="get-user" disabled>GetAllUserInfo With Jwt Token</button>
        <div id="user"></div>
    </form>
</body>
</html>
