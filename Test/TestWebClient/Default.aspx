<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="TestWebClient._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">

        var splitString = function (s) {
            var splitChar = " ";
            if (s.indexOf(",") > 0) {
                splitChar = ",";
            }
            var arr = s.split(splitChar);
            if (arr.length > 1) {
                var x = arr[0];
                arr.splice(0, 1);
                return [x, arr.join(splitChar)];
            } else {
                return [s,s];
            }
        };

        var x = "Diane Marie Lundegård";
        var y = "silberg,Robin";
        var z = "silberg";

        var result1 = splitString(x);
        var result2 = splitString(y);
        var result3 = splitString(z);
        var xfd = 343;

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Test">
        </div>
    </form>
</body>
</html>
