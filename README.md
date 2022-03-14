# Atv - Jogos-Digitais

Desafio Individual Unity da disciplina de Jogos Digitais do período 2021.1 da UFRPE

# Mecânicas do jogador
 - Vida
   - O jogador inicia com 5 de vida, que decai ao entrar em contato com os inimigos ou cair do mapa
 - Movimentação
   - O jogador pode se movimentar pelo cenário utilizando as teclas **W**, **D** e **espaço**
 - Atirar
   - O jogador inicia com 10 balas que podem ser atiradas na direção do clique do mouse com intervalos de 200ms
 - Recarregar
   - Ao gastar todas as balas e tentar atirar ou apertar a tecla **R**, o jogador passa 500ms recarregando as balas, sem poder atirar no processo
 - Invencibilidade
   - Ao tomar dano, o jogador fica invencível por 1s

# Inimigos
 - Movimentação
   - Os inimigos se movimentam em direção ao jogador com velocidade dependente do tipo de inimigo, caso o jogador esteja acima dos inimigos e eles estiverem próximos o suficiente, os inimigos pulam em direção ao jogador.
 - Spawn
   - O spawn dos inimigos depende da onda atual do jogo, iniciando com 2 inimigo a cada 3s (1 por spawner).
 - Tipos
   - Zombie
     - Inimigo mais lento, porém sobrevive mais aos ataques do jogador, aparece desde a primeira onda.
   - Esqueleto
     - Possue a mesma velocidade que o jogador, mas possue 1 de vida a menos que o esqueleto. Aparece após a 6 onda.

# Ondas
Cada onda dura 10s e a cada onda a dificuldade do jogo aumenta. A cada onda os inimigos surgem 100ms mais rápido que a onda anterior. Alguns inimigos só surgem a partir de certa onda e podem ficar mais frequentes com o tempo.

# Cenário
O cenário possui três plataformas que o jogador pode se movimentar. As plataformas podem ser acessadas pulando sobre elas, ou por baixo. Os imigos são spawnados nas laterais da plataforma inferior. É possível que o jogador passe pelas extremidades do mapa, fazendo-o cair e, após um tempo, teleportado para o meio do mapa com ivencibilidade e menos 1 de vida.
