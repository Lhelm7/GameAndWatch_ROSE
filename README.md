Nom du jeu : Colors Flash

Concept : 

Jeu type game and watch, dans lequel le joueur va tenter de lancer une partie d’un jeu nommé “colors flash” seulement la borne d’arcade semblera défectueuse (effet dans le mainmenu) 
et le joueur devra parvenir à la réparer afin de pouvoir jouer au vrai jeu. Mais ainsi les jeux seront liés entre eux de par cette idée de réparation de la borne. Mais également tous les jeux seront autour des "couleurs".
Le premier jeu est un jeu de collect dans lequel des composants de différentes couleurs vont tomber dans des tuyaux et ainsi finirent leurs courses à différents endroits, 
et le player représenté par deux couleurs devra récupérer la bonne combinaison de couleurs (celle qu’il a sur lui) afin d’augmenter son score. 
S'il récupère une mauvaise couleur ou une couleur qu’il a déjà récolté il perd alors une vie. Le jeu prend fin quand le joueur perd ses trois vies. 
Ainsi le but est de parvenir à faire le plus grand score. 

Mécanique principale : 

Récupérer la bonne combinaison de couleurs.  

Fonctionnement globale : 

Le jeu fonctionne par vague. A chaque vague, 2 composants de couleurs aléatoire spawn. 
Le joueur devra alors : récupérer les couleurs qu’il l’intéresse, et esquiver celle qu’il ne doit pas récupérer. 
Et par la suite attendre la prochaine vague. Une fois que le joueur effectue une bonne combinaison de couleurs il gagne 10 points, et ainsi de suite. 
À partir de 30 points au score le jeu augmente en difficulté. La vitesse des components et leur vitesse de spawn augmentent. 
Les component spawn plus vite, et augmentent en nombre (à 30 points => 3 components spawn / 60 = 4 composants spawn / 90 = 5 composants). 
Ainsi la difficulté et progressive au fil du jeu. 

Feeling :  

Pour améliorer le game feel, quand le joueur récupère une bonne couleur une petite animation venant du player se joue ainsi qu’un feedback de son, 
et également la couleur présente sur le player devient opaque permettant de faire comprendre que cette couleur a déjà été récupéré. 

Également pour améliorer le feeling, des petites animations ont été faite sur les boutons, lorsque le joueur clique. 

Fonctionnalités principales : 

Ainsi pour le moment ce qui compose le plus gros et le plus important du projet est ; 
Le game manager, pour gérer les vagues de component ; L’UI manager, pour gérer tous ce qui touche à l’UI (le score, la vie du player...etc) ; 
Le player collector, qui gèrent enregistrent les composants qui ont été collectés ; Componentcollectable, les composants que le joueur récupère ; 
Player movement, qui gère les mouvements du player. 
