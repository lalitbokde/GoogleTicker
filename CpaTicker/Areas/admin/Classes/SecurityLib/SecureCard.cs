﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace CpaTicker.Areas.admin.Classes.SecurityLib
{
    public class SecureCard
    {
        private bool isDecrypted = false;
        private bool isEncrypted = false;
        private string cardNumber;
        private string expiryMonth;
        private string expiryYear;
        private string cvv;

        /// <summary>
        /// this fied is all calculated by the number so in the future it may be remove
        /// </summary>
        private string cardType;
        private string encryptedData;
        private XmlDocument xmlCardData;

        public SecureCard(string newEncryptedData)
        {
            // constructor for use with encrypted data
            encryptedData = newEncryptedData;
            DecryptData();
        }

        public SecureCard(string newCardNumber, string expMonth,
          string expYear, string cvv,
          string newCardType)
        {
            // constructor for use with decrypted data      
            cardNumber = newCardNumber;
            expiryMonth = expMonth;
            expiryYear = expYear;
            this.cvv = cvv;
            cardType = newCardType;
            EncryptData();
        }

        public SecureCard(string newCardNumber, string expMonth, string expYear, string cvv)
        {
            // constructor for use with decrypted data   
            isDecrypted = true;
            cardNumber = newCardNumber;
            expiryMonth = expMonth;
            expiryYear = expYear;
            this.cvv = cvv;
            cardType = GetCCTypeFromNumber(cardNumber);
            EncryptData();
        }

        private string GetCCTypeFromNumber(string cardNumber)
        {
            if (Regex.IsMatch(cardNumber, "^(4)"))
                return "visa";
            if (Regex.IsMatch(cardNumber, "^(5)"))
                return "master";
             if (Regex.IsMatch(cardNumber, "^(37)"))
                 return "amex";
             if (Regex.IsMatch(cardNumber, "^(6011)"))
                 return "discover";
            return "Unknown";
        }

        private void CreateXml()
        {
            // encode card details as XML document
            xmlCardData = new XmlDocument();
            XmlElement documentRoot =
              xmlCardData.CreateElement("CardDetails");
            XmlElement child;

            child = xmlCardData.CreateElement("CardNumber");
            child.InnerXml = cardNumber;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("ExpiryMonth");
            child.InnerXml = expiryMonth;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("ExpiryYear");
            child.InnerXml = expiryYear;
            documentRoot.AppendChild(child);


            child = xmlCardData.CreateElement("CVV");
            child.InnerXml = cvv;
            documentRoot.AppendChild(child);

            child = xmlCardData.CreateElement("CardType");
            child.InnerXml = cardType;
            documentRoot.AppendChild(child);
            xmlCardData.AppendChild(documentRoot);
        }

        private void ExtractXml()
        {
            // get card details out of XML document
            cardNumber =
              xmlCardData.GetElementsByTagName(
                "CardNumber").Item(0).InnerXml;
            expiryMonth =
              xmlCardData.GetElementsByTagName(
                "ExpiryMonth").Item(0).InnerXml;
            expiryYear =
              xmlCardData.GetElementsByTagName(
                "ExpiryYear").Item(0).InnerXml;
            cvv =
              xmlCardData.GetElementsByTagName(
                "CVV").Item(0).InnerXml;
            cardType =
              xmlCardData.GetElementsByTagName(
                "CardType").Item(0).InnerXml;
        }

        private void EncryptData()
        {
            try
            {
                // put data into XML doc
                CreateXml();
                // encrypt data
                encryptedData =
                  StringEncryptor.Encrypt(xmlCardData.OuterXml);
                // set encrypted flag
                isEncrypted = true;
            }
            catch
            {
                throw new SecureCardException("Unable to encrypt data.");
            }
        }

        private void DecryptData()
        {
            try
            {
                // decrypt data
                xmlCardData = new XmlDocument();
                xmlCardData.InnerXml =
                  StringEncryptor.Decrypt(encryptedData);
                // extract data from XML
                ExtractXml();
                // set decrypted flag
                isDecrypted = true;
            }
            catch
            {
                throw new SecureCardException("Unable to decrypt data.");
            }
        }

        public string CardNumber
        {
            get
            {
                if (isDecrypted)
                {
                    return cardNumber;
                }
                else
                {
                    throw new SecureCardException("Data not decrypted.");
                }
            }
        }

        public string CardNumberX
        {
            get
            {
                if (isDecrypted)
                {
                    return "XXXX-XXXX-XXXX-"
                       + cardNumber.Substring(cardNumber.Length - 4, 4);
                }
                else
                {
                    throw new SecureCardException("Data not decrypted.");
                }
            }
        }

        public string ExpiryMonth
        {
            get
            {
                if (isDecrypted)
                {
                    return expiryMonth;
                }
                else
                {
                    throw new SecureCardException("Data not decrypted.");
                }
            }
        }

        public string ExpiryYear
        {
            get
            {
                if (isDecrypted)
                {
                    return expiryYear;
                }
                else
                {
                    throw new SecureCardException("Data not decrypted.");
                }
            }
        }

        public string CVV
        {
            get
            {
                if (isDecrypted)
                {
                    return cvv;
                }
                else
                {
                    throw new SecureCardException("Data not decrypted.");
                }
            }
        }

        public string CardType
        {
            get
            {
                if (isDecrypted)
                {
                    return cardType;
                }
                else
                {
                    throw new SecureCardException("Data not decrypted.");
                }
            }
        }

        public string EncryptedData
        {
            get
            {
                if (isEncrypted)
                {
                    return encryptedData;
                }
                else
                {
                    throw new SecureCardException("Data not decrypted.");
                }
            }
        }

        public string GetCardType
        { 
            get 
            {
                return this.GetCCTypeFromNumber(CardNumber);
            } 
        }
    }
}