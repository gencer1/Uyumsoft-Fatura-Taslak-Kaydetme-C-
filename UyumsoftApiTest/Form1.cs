using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UyumsoftApiTest {
    public partial class Form1:Form {


        private Uyumsoft.BasicIntegrationClient client;
        private Uyumsoft.UserInformation userInformation;
        public Form1() {
            InitializeComponent();
            client = new Uyumsoft.BasicIntegrationClient();
            //SetBasicIntegrationSettingsForTest();
            SetCredential();
        }

        private void SetCredential() {
            // Canlı wsdle bağlanırken bu yöntemin kullanılması gerekiyor.
            userInformation = new Uyumsoft.UserInformation() { Username = "kullanıcıAdı", Password = "şifre" };
        }

        private void SetBasicIntegrationSettingsForTest() {
            // Test wsdline bağlanırken bu yöntemin kullanılması gerekiyor.
            client.ClientCredentials.UserName.UserName = "kullanıcıAdı";
            client.ClientCredentials.UserName.Password = "şifre";
        }

        private void button1_Click(object sender, EventArgs e) {
            // Mükelleft e-fatura mı e arşiv mi sorgusu gerekirse bu kodu kullanabilirsiniz.
            //Uyumsoft.FlagResponse isEfaturaResponse = client.IsEInvoiceUser(userInformation,"VKN veya TCKN", "");

            Uyumsoft.InvoiceType invoiceType = new Uyumsoft.InvoiceType();
            #region Fatura Bilgileri
            invoiceType.UBLVersionID = new Uyumsoft.UBLVersionIDType() { Value = "TR1.2" };
            invoiceType.ProfileID = new Uyumsoft.ProfileIDType() { Value = "TICARIFATURA" };
            invoiceType.CopyIndicator = new Uyumsoft.CopyIndicatorType() { Value = false };
            invoiceType.IssueDate = new Uyumsoft.IssueDateType() { Value = DateTime.Now };
            invoiceType.IssueTime = new Uyumsoft.IssueTimeType() { Value = DateTime.Now };
            invoiceType.InvoiceTypeCode = new Uyumsoft.InvoiceTypeCodeType() { Value = "SATIS" };
            invoiceType.Note = new Uyumsoft.NoteType[] { new Uyumsoft.NoteType() { Value = "Fatura Notu -1" } };
            invoiceType.DocumentCurrencyCode = new Uyumsoft.DocumentCurrencyCodeType() { Value = "TRY" };
            invoiceType.LineCountNumeric = new Uyumsoft.LineCountNumericType() { Value = 1 };
            #endregion


            #region Mükellef Bilgileri
            Uyumsoft.PartyType partyType = new Uyumsoft.PartyType() {
                PartyIdentification = new Uyumsoft.PartyIdentificationType[] {
                    new Uyumsoft.PartyIdentificationType() { ID = new Uyumsoft.IDType() { schemeID = "TCKN", Value = "Firma sahibinin VKN veya TCKN" } },
                    new Uyumsoft.PartyIdentificationType() { ID = new Uyumsoft.IDType(){ schemeID = "MERSISNO", Value = "" } },
                    new Uyumsoft.PartyIdentificationType() { ID = new Uyumsoft.IDType() { schemeID = "TICARETSICILNO", Value = "" } },
                },
                PartyName = new Uyumsoft.PartyNameType() { Name = new Uyumsoft.NameType1 { Value = "https://virtuozsoftware.com" } },
                PostalAddress = new Uyumsoft.AddressType() {
                    StreetName = new Uyumsoft.StreetNameType() { Value = "Firma adresi" },
                    CitySubdivisionName = new Uyumsoft.CitySubdivisionNameType() { Value = "menteşe" },
                    CityName = new Uyumsoft.CityNameType() { Value = "Muğla" },
                    Country = new Uyumsoft.CountryType() { Name = new Uyumsoft.NameType1() { Value = "Türkiye" } }
                },
                PartyTaxScheme = new Uyumsoft.PartyTaxSchemeType() { TaxScheme = new Uyumsoft.TaxSchemeType() { Name = new Uyumsoft.NameType1() { Value = "Menteşe" } } },
                Person = new Uyumsoft.PersonType() { FirstName = new Uyumsoft.FirstNameType() { Value = "FirmaSahibiAdı" }, FamilyName = new Uyumsoft.FamilyNameType() { Value = "FirmaSahibiSoyadı" } }
            };
            invoiceType.AccountingSupplierParty = new Uyumsoft.SupplierPartyType() { Party = partyType };
            #endregion


            Uyumsoft.CustomerPartyType customerParty = new Uyumsoft.CustomerPartyType() {
                Party = new Uyumsoft.PartyType() {
                    PartyIdentification = new Uyumsoft.PartyIdentificationType[] { new Uyumsoft.PartyIdentificationType() { ID = new Uyumsoft.IDType() { schemeID = "TCKN", Value = "46093695518" } } },
                    Person = new Uyumsoft.PersonType() { FirstName = new Uyumsoft.FirstNameType() { Value = "Gencer" }, FamilyName = new Uyumsoft.FamilyNameType() { Value = "Get" } },
                    PostalAddress = new Uyumsoft.AddressType() {
                        Room = new Uyumsoft.RoomType() { Value = "" },
                        StreetName = new Uyumsoft.StreetNameType() { Value = "Hacı Ahmet Sokak." },
                        BuildingNumber = new Uyumsoft.BuildingNumberType() { Value = "1" },
                        CitySubdivisionName = new Uyumsoft.CitySubdivisionNameType() { Value = "Menteşe" },
                        CityName = new Uyumsoft.CityNameType() { Value = "Muğla" },
                        Country = new Uyumsoft.CountryType() { Name = new Uyumsoft.NameType1() { Value = "Türkiye" } }
                    },
                    PartyTaxScheme = new Uyumsoft.PartyTaxSchemeType() { TaxScheme = new Uyumsoft.TaxSchemeType() { Name = new Uyumsoft.NameType1(){ Value = "" } } },
                    Contact = new Uyumsoft.ContactType() { Telephone = new Uyumsoft.TelephoneType() { Value = "05418722234" } }
                }
            };
            invoiceType.AccountingCustomerParty = customerParty;

            #region Taksit Bilgileri
            invoiceType.TaxTotal = new Uyumsoft.TaxTotalType[] {
                new Uyumsoft.TaxTotalType(){
                    TaxAmount = new Uyumsoft.TaxAmountType(){ currencyID = "TRY", Value = 18 },
                    TaxSubtotal = new Uyumsoft.TaxSubtotalType[]{
                        new Uyumsoft.TaxSubtotalType(){
                            TaxableAmount = new Uyumsoft.TaxableAmountType() { currencyID = "TRY",Value= 18 },
                            TaxAmount = new Uyumsoft.TaxAmountType() { currencyID = "TRY" , Value=18},
                            Percent = new Uyumsoft.PercentType1() { Value = 18},
                            TaxCategory = new Uyumsoft.TaxCategoryType() {
                                TaxScheme = new Uyumsoft.TaxSchemeType() {
                                    Name = new Uyumsoft.NameType1() { Value = "KDV"} ,
                                    TaxTypeCode = new Uyumsoft.TaxTypeCodeType() { Value = "0015" }
                                }
                            }
                        }
                    }
                },

            };
            #endregion


            #region Faturanın genel ücret bilgileri
            invoiceType.LegalMonetaryTotal = new Uyumsoft.MonetaryTotalType() {
                LineExtensionAmount = new Uyumsoft.LineExtensionAmountType() { currencyID = "TRY", Value = 100 }, //Toplam ana tutar
                TaxExclusiveAmount = new Uyumsoft.TaxExclusiveAmountType() { currencyID = "TRY", Value = 82 }, //Vergisiz toplam
                TaxInclusiveAmount = new Uyumsoft.TaxInclusiveAmountType() { currencyID = "TRY", Value = 100 }, //Vergili toplam tutar
                AllowanceTotalAmount = new Uyumsoft.AllowanceTotalAmountType() { currencyID = "TRY" },
                PayableAmount = new Uyumsoft.PayableAmountType() { currencyID = "TRY", Value = 100 } //Ödenecek toplam tutar
            };
            #endregion


            #region Faturaya ait ürünlerin bilerileri (Array)
            invoiceType.InvoiceLine = new Uyumsoft.InvoiceLineType[] {
                new Uyumsoft.InvoiceLineType(){
                    ID = new Uyumsoft.IDType() { Value = "1"},
                    Note = new Uyumsoft.NoteType[]{ new Uyumsoft.NoteType() { Value = "Ürün Note" } },
                    InvoicedQuantity = new Uyumsoft.InvoicedQuantityType(){ unitCode = "NIU", Value = 1 },
                    LineExtensionAmount = new Uyumsoft.LineExtensionAmountType(){ currencyID = "TRY", Value=82 },
                    TaxTotal = new Uyumsoft.TaxTotalType(){
                        TaxAmount = new Uyumsoft.TaxAmountType(){ currencyID = "TRY", Value = 18 },
                        TaxSubtotal = new Uyumsoft.TaxSubtotalType[]{
                            new Uyumsoft.TaxSubtotalType(){
                                TaxableAmount = new Uyumsoft.TaxableAmountType() { currencyID = "TRY",Value= 18 },
                                TaxAmount = new Uyumsoft.TaxAmountType() { currencyID = "TRY" , Value=18},
                                Percent = new Uyumsoft.PercentType1() { Value = 18},
                                TaxCategory = new Uyumsoft.TaxCategoryType() {
                                    TaxScheme = new Uyumsoft.TaxSchemeType() {
                                        Name = new Uyumsoft.NameType1() { Value = "KDV"} ,
                                        TaxTypeCode = new Uyumsoft.TaxTypeCodeType() { Value = "0015" }
                                    }
                                }
                            }
                        }
                    },
                    Item = new Uyumsoft.ItemType(){
                        Description = new Uyumsoft.DescriptionType() {Value ="Ürün açıklaması"},
                        Name = new Uyumsoft.NameType1() {Value="Web sitesi düzenleme ve geliştirme"},
                        ModelName = new Uyumsoft.ModelNameType() {Value="" }
                    },
                    Price = new Uyumsoft.PriceType(){
                        PriceAmount = new Uyumsoft.PriceAmountType() {
                            currencyID = "TRY",
                            Value = 82
                        }
                    }
                }
            };
            #endregion


            #region Fatura ile ilgili diğer bilgiler
            Uyumsoft.InvoiceInfo infos = new Uyumsoft.InvoiceInfo();
            infos.Invoice = invoiceType;
            infos.EArchiveInvoiceInfo = new Uyumsoft.EArchiveInvoiceInformation() { DeliveryType = Uyumsoft.InvoiceDeliveryType.Electronic };
            infos.Scenario = Uyumsoft.InvoiceScenarioChoosen.Automated;
            infos.Notification = new Uyumsoft.NotificationInformation() {
                Mailing = new Uyumsoft.MailingInformation[] {
                    new Uyumsoft.MailingInformation(){
                        Subject = "GG - Fatura Oluşturuldu",
                        EnableNotification = true,
                        To = "gen-cer@hotmail.com",
                        Attachment = new Uyumsoft.MailAttachmentInformation() { Xml=true, Pdf=true}
                    }
                }
            };
            infos.LocalDocumentId = "E-Fatura 00001";
            #endregion

            var request = new Uyumsoft.InvoiceInfo[] { infos };

            #region Request XML Üretme
            /*
             * Farklı bir dilde kullanmak vb. amaçlar için requestin xml halini elde etmek istiyorsanız aşağıdaki kodları açabilirsiniz.
             * Xml adlı string içerisine yüklenecektir !
            
            var serxml = new System.Xml.Serialization.XmlSerializer(request.GetType());
            var ms = new MemoryStream();
            serxml.Serialize(ms, request);
            string xml = Encoding.UTF8.GetString(ms.ToArray());
             */
            #endregion

            var response = client.SaveAsDraft(userInformation, request);

            textBox1.Text = response.Message;
        }

    }
}
