<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OAuthWcfApp.WebFormClient.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WCF Service Client</title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#login').click(function (event) {
                event.preventDefault();
                var inputLogin = $('#login-input').val();
                var inputPassword = $('#password-input').val();
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/AuthorizeUser",
                    data: JSON.stringify({ login: inputLogin, password: inputPassword }),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var response = data.d;
                        $('#jwtToken').text("");
                        if (response.Success) {
                            $('#accessToken').text(response.Token);
                            $('#login-error').text("");
                            $('#login-approved').text("Login approved: " + response.Token);
                            $('#get-jwtToken').removeAttr('disabled');
                        } else {
                            $('#accessToken').text("");
                            $('#login-approved').text("");
                            $('#login-error').text("Login or password is not correct: " + response.Message);
                            $('#get-user').attr('disabled', 'disabled');
                        }
                    },
                    error: function (xhr, status, error) {
                        $('#accessToken').text("");
                        $('#login-error').text(error + " " + xhr.responseText);
                        $('#get-user').attr('disabled', 'disabled');
                    }
                });
            });
            $('#get-jwtToken').click(function (event) {
                event.preventDefault();
                var inputGrantToken = $('#accessToken').text();
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/ExchangeGrantToken",
                    data: JSON.stringify({ authorizationGrant: inputGrantToken }),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var response = data.d;
                        $('#jwtToken').text(response.JwtToken);
                        $('#login-error').text("");
                        $('#login-approved').text("");
                        $('#accessToken').text("");
                        $('#get-user').removeAttr('disabled');
                    },
                    error: function (xhr, status, error) {
                        $('#jwtToken').text("");
                        $('#login-error').text(error + " " + xhr.responseText);
                        $('#get-user').attr('disabled', 'disabled');
                    }
                });
            });
            $('#get-user').click(function (event) {
                event.preventDefault();
                var jwtToken = $('#jwtToken').text();
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/GetUserWithJwtToken",
                    data: JSON.stringify({ authorizedJwtToken: jwtToken }),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var response = data.d;
                        $('#user').text(JSON.stringify(response));
                    },
                    error: function (xhr, status, error) {
                        $('#user').text("");
                        ('#login-error').text(error + " " + xhr.responseText);
                        $('#get-user').attr('disabled', 'disabled');
                    }
                });
            });
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="smMain" runat="server" EnablePageMethods="true" />
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
            <div id="login-approved" style="color: green;"></div>
        </div>
        <br />
        <button id="get-jwtToken" disabled>Exchange grant token to Jwt Token</button>
        <div id="jwtToken"></div>
        <button id="get-user" disabled>GetAllUserInfo With Jwt Token</button>
        <div id="user"></div>
    </form>
</body>
</html>
