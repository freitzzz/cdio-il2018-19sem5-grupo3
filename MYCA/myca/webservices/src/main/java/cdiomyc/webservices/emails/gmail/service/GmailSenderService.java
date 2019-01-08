package cdiomyc.webservices.emails.gmail.service;

import java.util.Properties;
import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import cdiomyc.webservices.emails.sendgrid.EmailSendDetailsMV;

/**
 * Service that sends an email using the Gmail SMTP
 * 
 * @author Gil Durao
 */
public class GmailSenderService {

    /**
     * String containing the type of email Carrier to use
     */
    public static final String CARRIER = "Gmail";

    /**
     * String containing the Gmail SMTP Port
     */
    private static final String GMAIL_SMTP_PORT = "587";

    /**
     * String containing the sender email address
     */
    private static final String FROM_USER = "makeyourclosetsender@gmail.com";

    /**
     * String containing the sender email address (Should be encrypted)
     */
    private static final String EMAIL_PASSWORD = "MakeYourCloset123";

    /**
     * Sends an email using the Gmail SMTP
     * 
     * @param emailSendDetailsMV details of the email being sent
     * 
     */
    public static void send(EmailSendDetailsMV emailSendDetailsMV) {

        Properties emailProperties = System.getProperties();

        emailProperties.put("mail.smtp.port", GMAIL_SMTP_PORT);
        emailProperties.put("mail.smtp.auth", "true");
        emailProperties.put("mail.smtp.starttls.enable", "true");   

        Session mailSession = Session.getDefaultInstance(emailProperties);
        MimeMessage emailMessage = new MimeMessage(mailSession);

        try {

            for (int i = 0; i < emailSendDetailsMV.receptorsEmails.length; i++) {
                emailMessage.addRecipient(Message.RecipientType.TO,
                        new InternetAddress(emailSendDetailsMV.receptorsEmails[i]));
            }

            emailMessage.setSubject(emailSendDetailsMV.title);
            emailMessage.setContent(emailSendDetailsMV.message, "text/html");

            String emailHost = "smtp.gmail.com";

            Transport transport = mailSession.getTransport("smtp");

            transport.connect(emailHost, FROM_USER, EMAIL_PASSWORD);
            transport.sendMessage(emailMessage, emailMessage.getAllRecipients());
            transport.close();

        } catch (MessagingException e) {
            throw new IllegalStateException("An error occurred when sending the e-mail.");
        }
    }
}