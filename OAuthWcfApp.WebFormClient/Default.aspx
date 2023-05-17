<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OAuthWcfApp.WebFormClient.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>WCF Service Client</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#login').click(function (event) {
                event.preventDefault();
                var login = $('#login-input').val();
                var password = $('#password-input').val();

                var loginPasswordData = { Login: login, Password: password };
                var loginPasswordJson = JSON.stringify(loginPasswordData);

                // Create SOAP message for WCF
                var soapRequest =
                    '<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" ' +
                    'xmlns:tem="http://tempuri.org/">' +
                    '<soapenv:Header/>' +
                    '<soapenv:Body>' +
                    '<tem:Authorize>' +
                    '<tem:loginPasswordInJsonFormat>' + loginPasswordJson + '</tem:loginPasswordInJsonFormat>' +
                    '</tem:Authorize>' +
                    '</soapenv:Body>' +
                    '</soapenv:Envelope>';

                $.ajax({
                    type: "POST",
                    url: "Default.aspx/AuthorizeUser",
                    data: soapRequest,
                    contentType: "text/xml; charset=utf-8",
                    dataType: "xml",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('SOAPAction', 'http://tempuri.org/ITokenService/Authorize');
                    },
                    success: function (data) {
                        var response = JSON.parse($(data).find("AuthorizeResult").text());

                        if (response.success) {
                            $('#accessToken').text(response.token);
                            $('#login-error').text("");
                            $('#login-error').text("Login approved: " + response.token);
                            $('#get-user').removeAttr('disabled');
                        } else {
                            $('#accessToken').text("");
                            $('#login-error').text("Login failed: " + response.error);
                            $('#get-user').attr('disabled', 'disabled');
                        }
                    },
                    error: function (xhr, status, error) {
                        $('#accessToken').text("");
                        $('#login-error').text("Login failed: " + error + " " + xhr.responseText);
                        $('#get-user').attr('disabled', 'disabled');
                    }
                });
            });

            $('#get-user').click(function (event) {
                event.preventDefault();
                var accessToken = $('#accessToken').text();

                $.ajax({
                    type: "POST",
                    url: "Default.aspx/GetAllUserInfo",
                    data: "{ accessToken: '" + accessToken + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d.success) {
                            var user = JSON.parse(data.d.user);
                            $('#user').text("User ID: " + user.Id + ", Login: " + user.Login);
                        } else {
                            $('#user').text("Error: " + data.d.error);
                        }
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
