Read me other language :  [Türkçe](https://github.com/TufanOzdemir/ibmmqsample/blob/master/README.md), [English](https://github.com/TufanOzdemir/ibmmqsample/blob/master/README.en.md)

Bu proje şuan için sadece ibmmq için temel kodlar içerse de diğer mq servislerini de test etmek için uygun bir alt yapı sağlar. Kurulum ve kullanım için wiki ekranlarını inceleyebilirsiniz. 
1. [Docker için kurulum](https://github.com/TufanOzdemir/ibmmqsample/wiki/Kurulum)
1. [Kullanım](https://github.com/TufanOzdemir/ibmmqsample/wiki/Kullan%C4%B1m)

### Testler ve genel görünüm
Bu testler yine uygulama içinde 3 farklı kopya üzerinde yapılmış biri üretici veya yayıncı, diğerleri ise tüketici veya abone konumunda olmuşlardır.
Standart Queue yapısı ekleme ve dağıtım düzenli şekilde her seferinde başka yere gidecek şekilde istekleri bölmektedir.

![Standart Queue](https://github.com/TufanOzdemir/ibmmqsample/blob/master/IbmMQSample/wiki/QueuePutGet.png)

Pub/Sub gelen isteği o an kanala bağlı tüm clientlara dağıtmaktadır.

![Pub/Sub](https://github.com/TufanOzdemir/ibmmqsample/blob/master/IbmMQSample/wiki/QueuePubSub.png)

