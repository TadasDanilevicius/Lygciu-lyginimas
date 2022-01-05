# Lygciu lyginimas
tai yra Console aplikacija, kuri gali išlyginti cheminių reakcijų lygtis.

# Naudojimosi taisyklės
1) junginiai yra atskiriami pliusais '+', o reakcijos rodyklė užrašoma ženklu '=>'  
2) junginių viduje negali būti tarpų  
<br>Reakcijų pavyzdžiai duoti faile 'reakcijos.txt'
<br>

![alt text](https://github.com/TadasDanilevicius/Lygciu-lyginimas/blob/master/Console%20screen.png)

# Kaip Veikia programa
1) Programa atpažįsta reakcijoje dalivaujančius elementus ir sudeda į 'List\<string\> elementai' (Elementai eina iš kairės žiūrint į reakcijos lygtį).
2) Suskaičiuoja kiekviename junginyje esančių elementų kieki (skaičius neigiamas jei tai reagentas) ir sukuria 'matrica', kurioje pirma dimensija yra elementai iš prieš tai sukurto sąrašo, o antroje dimensijoje - 
junginio numeris. Matrica padauginus is vektoriu [a; b; c] visų eilučių suma turi būti lygi nuliui.
Pvz.:
> *A* H2O2 => *B* H2O + *C* O2  
H&emsp;2&emsp;&emsp;&emsp;-2&emsp;&emsp;&emsp;0  
O&emsp;2&emsp;&emsp;&emsp;-2&emsp;&emsp;&emsp;-2  
elementai = [H O]  
matrica =  
[2 -2 0;  
&nbsp;2 -1 -2]
3) sudaryta matrica sprendžima Gausso metodu: 
a) eilutės su nuliais pirmame stulpelyje perkeliamos į apačią. 
b) Surandamas pirmojo stulpelio bendras kartotinis.
c) Sudauginamos eilutės taip, kad pirmo stulpelio koeficientai tarpų vienodi ir neigiami (išskyrus pirma eilutė teigiama).
d) Pirma eilutė sudedama su kitomis eilutėmis.
e) Eilutės išprastinamos radus mažiausią bendrą daliklį.
f) Tie patys veiksmai kartojami su likusia matrica, kol lieka du du stulpeliai.
Pvz.:
> Išsprendus Gausso metodu lygtį gaunama trapecinė matrica;
m =  
[1 -1 0;  
&nbsp;0 -1 2]
4) Jei matrica netapo trapecine, reakcijos lygtis sprendinių neturi. Jei trapecinės matricos apatinėje eilutėje yra daugiau nei du nariai, tai sprendinių yra daugiau nei vienas.
Dabar iš eilės nuo matricos dešinio apatinio kampo galima skaičiuoti koeficientus. Anksčiau rastus koeficientus išreiškiame per paskutinį koeficientą.
> Kadangi reakcijos lygties koeficientai yra santykiniai dydžiai ir galima užrašyti kanoniniu pavidalu.  
A/koeficientai(1) = B/koeficientai(2) = C/koeficientai(2)
5) gautą koeficientų vektorių lieka padalinti iš mažiausio bendro daliklio ir gauname sprendinį.
> Šiuo atveju koeficientai = [2 2 1]
