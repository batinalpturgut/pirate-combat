**TODO:** Tower'lari TowerPrototype prefab'inden uret. Visual'lar ATowerSO'da tutulsun. Su anda FreezingTowerSO'da
FreezingTower prefab'i yuklu. Bunu TowerPrototype prefab'i ile degistir.

**TODO:** Ship'leri ShipPrototype prefab'inden uret. Visual'lar AShipSO'da tutulsun. Su anda SO'larda Archer ve Heavy
prefab'leri yuklu. Bunları ShipPrototype ile degistir.

**TODO:** Kaptan, Tower ve Ship icin uretici, birlestici class'lar yazilacak.

**Fikir:** RaritySystemSO gibi bir yapiyi Tower ve Ship'ler icin uygulamak. InventoryManager'in AShipSO[] array'i
tutması yerine sadece ShipSOSystem gibi bir yapi tutabilir. ShipSOSystem AShipSO[] array'i tutsun.
Ayni sey TowerSO'lar icin de gecerli. Yapilabilir mi?

**Bug**: Node dolu olmasina ragmen deployable eklenmeye calisildiginda soul harcaniyor.


