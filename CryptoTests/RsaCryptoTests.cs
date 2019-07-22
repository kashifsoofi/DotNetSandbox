using System;
using Crypto;
using Shouldly;
using Xunit;

namespace CryptoTests
{
    public class RsaCryptoTests
    {
        private readonly RsaCrypto rsaCrypto;

        public RsaCryptoTests()
        {
            rsaCrypto = new RsaCrypto();
        }

        [Fact]
        public void CryptoTest()
        {
            var plainText = "TEST";

            var encyptionKey = rsaCrypto.LoadPublic("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkdgLUK_Du7ZdGSoblJkWo5spPSV5IZVb75f8VqbSf8AwI214bhFeOLGrqInwEwscdKphlcxlmnx9ME-ABObPkExMq_CNd__WFIY--7mn6IxexMsx_ohsRscO1gZOh5SfwCOOhSCfKtouyfTSgv2c6JeT0utjIWFUawooXs983R1bqpXoMhwxMrIyFY7uR7gV47ar0JMPn74pna9tvq67cENnm4HXjA-yWMW5wc9lydVB5N7IjwU7LHFyBH0EimWxvSuRmc-nLpouqSYZAR5IQ2Lu28BEAnPL393cVI1_mt2JG6J9AkY-Q0pTh7Fr2tUcYVppRv9RwKh4YOYHabd37QIDAQAB");
            var encrypted = rsaCrypto.Encrypt(plainText, encyptionKey);

            var decryptionKey = rsaCrypto.LoadPrivate("MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCR2AtQr8O7tl0ZKhuUmRajmyk9JXkhlVvvl_xWptJ_wDAjbXhuEV44sauoifATCxx0qmGVzGWafH0wT4AE5s-QTEyr8I13_9YUhj77uafojF7EyzH-iGxGxw7WBk6HlJ_AI46FIJ8q2i7J9NKC_Zzol5PS62MhYVRrCihez3zdHVuqlegyHDEysjIVju5HuBXjtqvQkw-fvimdr22-rrtwQ2ebgdeMD7JYxbnBz2XJ1UHk3siPBTsscXIEfQSKZbG9K5GZz6cumi6pJhkBHkhDYu7bwEQCc8vf3dxUjX-a3Ykbon0CRj5DSlOHsWva1RxhWmlG_1HAqHhg5gdpt3ftAgMBAAECggEAVvQ6etbwmmB2TWSfoQ9NQipmggBvoihM8iLp3jgEVQqfKOBB504PoTm9IPVs383GH4DeQdPl2B_U_BLKPyHmKyByrij0D9HHL-GCd88PX0Uh069alWl9NQ3FuLF17LweKw6IELOMcp43O03unQ7cmIiXKDAToHlJPbCGtVB8H9BSE593uHJBhU0lYDYVBWHKE3ts2jU2830kgWLRLA3wAgJj6OdBTGEEjkN7x3H3hgRybd_xoi0mYYRrnFgZ_MaskegvTn4LOZqZqk9vZh7P7DQFhjMWdBPzH4V51X2PS2exLbracTd9b75gWcNh_nxPNINAgvbjteLNuZkxKcs4-QKBgQDhLxbnp0Ygqypja7LUlgKTfA0MFaaBm340hCHjhjMli_aQEBmvTriT4wC7x4aZIXnkBaeJG5OKRXKrLhKEX7B7SKoZSidWqLQMdOlPwWn3Q-dZmrt_UT0RMs5cQlzIT5Yf8l1DFjR156VHTgPdi3VIrmNBZOBbk3bwpgqsdWp-OwKBgQClzWvSuHL21i98fyV77vY6-GRsSfJTYzw_zGr-8IMN07jJr5PbAsrhggwNHno7ZONS9GohoIBZmn4e5m4gy4H7oWE29dn3B47D62bgP4T__IjsjAgAJcdKXuiFpZFLq4Q7kLv0WyWd6H5nxwaEHdQfAv9neO5lbpNmz8o8wfs39wKBgQCDIJuy69pPXb0CRg3N80iuv3cNiXH7WSOlyye8yUHxZE6A149NYYbkPzUHJAoCE9dZ690CXzeMNiKvAdYwlVQ8hjTfIypDMVwfQNk340Ykgbsvl4YFfrYT1MUMPmyvDIE8OzMJxN1ppym8mvZvRS1X1Izh4K8xRG7ndZkTkhAgTwKBgFFicN-hTEUfQ3Xfz11zIYg2rsx8y6bk2gkS5R44noul9lmBhpkFWOKyfAnggO0wi2kHsiTJbNcOv4OZZktQuX_zag2ZRiE8o3ZF0VyXsUgaBHfgEHlKEfXOemJHr_ctvJ2kYK4EI4XMPmfgSLGHFr0WMpnuwU4mpdHcgw-pxA9nAoGAQ5ss5wAnrCTb3Qlt7sErfaS16S2rC79bhIMl7I73-w_k5LsUDe7GKv4ZnJT0Fm_D27ajL1h2URJxjbxI1TRv7SKMztFfXx6YECkdWpzal02f8NJ300-0BNgBpQZ39EBbtGxS35RWS1qC-Dp8N52Qz35pK1mvP0Cff99oGTdZkjY");
            var decrypted = rsaCrypto.Decrypt(encrypted, decryptionKey);

            decrypted.ShouldBe(plainText);
        }
    }
}
