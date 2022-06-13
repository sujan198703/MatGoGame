var mailer = require("nodemailer");

var dbcon;
var admin_mail;
var admin_pass;

exports.send = send;
async function send(to, subject, text) {
    return new Promise(r => {
        mailer.createTransport({
            service: 'gmail',
            auth: {
              user: admin_mail,
              pass: admin_pass
            }
        }).sendMail(
            {
              from: admin_mail,
              to: to,
              subject: subject,
              text: text
            }, function(error, info) {
                if(error) r(false);
                else {
                    //console.log('Email send to ' + to + ", " + info.response);
                    r(true);
                }
            }
        );
    });
}

exports.init = async function (db) {
    dbcon = db;
    admin_mail = await dbcon.get_config_val("admin_mail");
    admin_pass = await dbcon.get_config_val("admin_pass");
}

exports.send_verification_code = async function(to, vcode) {
    return await send(to, "Email Verification", "Your verification code: " + vcode);
}

exports.send_verification_comfirm = async function(to) {
    return await send(to, "Email Verification", "Your email has been verified.");
}

exports.send_reset_code = async function(to, rcode) {
    return await send(to, "Reset Password", "Your reset code: " + rcode);
}

exports.send_reset_comfirm = async function(to) {
    return await send(to, "Reset Password", "Your password has been reset.");
}
