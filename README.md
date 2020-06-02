# ibmmqsample

Bu projeyi ayağa kaldırmak için bilgisayarınızda kurulu bir ibmmq yok ise şu yolu deneyin.
1. Bilgisayarınızda docker yoksa kurun.
1. Powershell üzerine ```docker pull ibmcom/mq:latest``` komutunu yazın.
   1. ```docker images``` komutunu girerek ibmcom/mq  latest image bilgisini görüntüleyebilirsiniz.
1. Volume oluşturunuz. ```docker volume create qm1data``` komutunu kullanabilirsiniz.
1. Network oluşturunuz. ```docker network create mq-demo-network``` komutunu kullanabilirsiniz.
1. ```docker run --env LICENSE=accept --env MQ_QMGR_NAME=QM1 --volume qm1data:/mnt/mqm --publish 1414:1414 --publish 9443:9443 --network mq-demo-network --network-alias qmgr --detach --env MQ_APP_PASSWORD=Ab123! ibmcom/mq:latest``` komutu ile çekilen image bilgisini container olarak ayağa kaldırın.

Bu şekilde artık bilgisayarınızda ibmmq çalışıyor vaziyette olacaktır. **Test etmek** için şu yolları izleyin.
1. Açılan container bilgisini ```docker ps``` ile görüntüleyiniz.
1. Container içine girin.  ```docker exec -ti <your-container-id> bin/bash```
1. ```dspmqver``` komutu ile versiyon bilgisi görebilirsiniz.
1. ```dspmq``` komutu ise anlık queue durumunu verir.

Proje için kullanım dökümanı için [buraya](https://github.com/TufanOzdemir/ibmmqsample/wiki/Kullan%C4%B1m) tıklayınız.
