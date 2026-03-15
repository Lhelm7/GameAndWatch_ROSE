Nom du jeu : Colors Flash

Jeu 1 — Pipe

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

Jeu 2 — Parasites

Concept :
En suivant cette idée de réparation de la borne et de jeux avec des couleurs, dans le deuxième jeu le joueur se retrouve face au système de la borne infecté par des parasites colorés qui perturbent le bon fonctionnement de la machine. Pour avancer, il devra ainsi les éliminer dans le bon ordre en s'appuyant sur sa mémoire.
Ce jeu reprend ainsi le concept du "Simon".

Mécanique principale :
Mémoriser la séquence de couleurs affichée dans l'UI, puis toucher les parasites correspondants dans le bon ordre avant que le temps ne soit écoulé.

Fonctionnement global :
Le jeu fonctionne par vagues définies via des ScriptableObjects (9 vagues au total). Au début de chaque vague, une séquence de couleurs s'affiche à l'écran puis disparaît. Et par la suite des parasites colorés spawnent alors sur l'écran à des positions aléatoires. Le joueur doit taper les parasites dans l'ordre exact de la séquence, et dans le temps imparti. Si le joueur touche le mauvais parasite de couleur ou si le timer atteint zéro, cela déclenche le game over. Chaque vague réussie rapporte 10 points et augmente la difficulté => la séquence s'allonge, le nombre de parasites à l'écran augmente, et le temps accordé se réduit. 

Feeling :
Un feedback sonore se déclenche à chaque fois que le joueur touche un bon parasite. Le temps restant au joueur s'affiche également à l'écran. Dés qu'ils sont touchés les parasites se destroy mais avant une petite animations se joue.

Fonctionnalités principales :
Ce qui compose le cœur du projet pour ce jeu est : le ParasiteGameManager, qui gère les vagues, le score, le timer et les conditions de victoire ou de défaite ; Ensuite le SequenceDisplayUI, qui gère l'affichage de la séquence à mémoriser ; le ParasiteSpawner, qui instancie/fait spawn les parasites aux emplacements prédéfini, avec les bonnes couleurs ; le Parasite, qui gère la détection du touch via IPointerClickHandler et communique la couleur touchée au manager ; et les WaveData ScriptableObjects, qui définissent pour chaque vague la durée, le nombre de parasites et le temps accordé aux joueurs.


Jeu 3 — Higher

Concept :
Le troisième jeu représente l'étape finale de la réparation de la borne. Le système est quasiment rétabli mais il est instable => les couleurs du player changent tout le temps. Le joueur incarne ainsi un petit personnage qui saute à l'infini sur des plateformes colorées. Ce dernier ne peut rebondir que sur les plateformes qui correspondent à sa couleur actuel.
Ce jeu est quant à lui inspiré du doodle Jump.

Mécanique principale :
Parvenir à aller le plus haut possible en rebondissant uniquement sur les plateformes de la même couleur que le joueur, en arrivant à anticiper les changements de couleur à venir.

Fonctionnement global :
Le joueur rebondit automatiquement sur les plateformes lorsqu'il les touche (uniquement par le dessus). Toutes les 5 secondes, sa couleur change automatiquement pour une nouvelle couleur différente de la précédente. Si le joueur atterrit sur une plateforme d'une couleur différente de la sienne, il meurt immédiatement. Les plateformes sont générées aléatoirement mais de manière contrôlé par un système de pool (50 objets pré-instanciés) et détruite lorsqu'elles passent sous la caméra. Elles apparaissent ainsi en lignes réparties horizontalement et bien espacé pour être sur que le joueur puisse passer. Les couleurs des plateformes apparaissent toujours équitablement les 4 afin qu'il y en ai toujours une  à proximité du player pour pas qu'il se retrouve bloqué. Et enfin le score du player correspond à la hauteur maximale atteinte (mètres).

Feeling :
La couleur suivante du player est affiché dans l'UI afin qu'il prenne compte de cela et s'adapte, anticipant ainsi ses sauts et déplacement. Un feedback sonore se joue quand le joueur saute (touche une bonne plateforme. Sur L'UI s'affiche également constamment et s'update constamment le score du joueur.

Fonctionnalités principales :
Ce qui compose le cœur du projet pour ce jeu est : le Script PlayerController, qui gère le déplacement horizontal, le wrap d'écran, la détection de collision avec les plateformes, le saut automatique et la mort du player; le Script ColorManager, qui s'occupe des changements de couleur du joueur à intervalle régulier ; le Script ColorSpawner, qui génère, et positionne les plateformes; le Script Platform, qui porte la couleur ou l'état neutre d'une plateforme et met à jour son visuel ; le ScoreManager, qui traque la hauteur maximale atteint par le joueur ; le script ColorChangeUI, qui affiche le timer, et la prévisualisation de la prochaine couleur du player ; et le script MobileInputHandler, qui s'occupe des inputs.
