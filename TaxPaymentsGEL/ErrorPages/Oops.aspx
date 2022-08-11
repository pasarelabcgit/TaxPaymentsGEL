<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Oops.aspx.cs" Inherits="TaxPaymentsGEL.ErrorPages.Oops" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Error</title>
	<meta name="description" content=""/>
	<meta name="keywords" content=""/>
	<meta name="author" content="101 Software - Proyecto GEL"/>
	<meta http-equiv="cleartype" content="on"/>
	<link rel="shortcut icon" href="../Content/img/favicon.ico"/>
    <link href='http://fonts.googleapis.com/css?family=Open+Sans+Condensed:300' rel='stylesheet' type='text/css'/>

<style type="text/css">
html {
	-webkit-background-size: cover;
	-moz-background-size: cover;
	-o-background-size: cover;
	background-size: cover;
	background-attachment: fixed;
	background-image: url(../Content/img/bg.jpg);
	background-repeat: no-repeat;
	background-position: center center;
}
img {border: 0px;}
.error{
	float: left;
	height: auto;
	width: 60%;
	margin-top:10%;
	background-color: #FFF;
	border-top-color: #F00;
	border-top-width: 4px;
	border-top-style: solid;
	text-align:right;
	padding:20px;
	font-size:12px;
	color:#333;
	}
.error img{
	float:right;
	margin-left:20px;
	}
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	font-family: 'Open Sans Condensed', sans-serif;
}
</style>
</head>

<body>
    <form id="form1" runat="server">
   <div class="error"><img src="../Content/img/bancolombia-logo.png" height="87" />
<h1>ERROR</h1>
<h3>Lo sentimos, ha ocurrido en un error.</h3><br />
            <br />
    </form>
</body>
</html>