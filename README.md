# Atv - Jogos-Digitais

Desafio Individual Unity da disciplina de Jogos Digitais do período 2021.1 da UFRPE

# Mecânicas do jogador
 - Vida
   - O jogador inicia com 5 de vida, que decai ao entrar em contato com os inimigos
 - Andar
   - O jogador pode se movimentar pelo cenário utilizando as teclas **W**, **A**, **S**, **D**
 - Pular
   - O jogador pode apertar espaço para pular
 - Atirar
   - O jogador inicia com 10 balas que podem ser atiradas na direção do clique do mouse com intervalos de 200ms
 - Recarregar
   - Ao gastar todas as balas e tentar atirar ou apertar **R**, o jogador passa 500ms recarregando as balas, sem poder atirar no processo
 - Invencibilidade
   - Ao tomar dano, o jogador fica invencível por 1s

# Inimigos
 - Movimentação
   - Os inimigos se movimentam em direção ao jogador com velocidade dependente do tipo de inimigo, caso o jogador esteja acima dos inimigos e eles estiverem próximos o suficiente, os inimigos pulam em direção ao jogador.
 - Vida
   - Cada tipo de inimigo possue uma quantidade de vida diferente

# Cenário
O cenário possui três plataformas que o jogador pode se movimentar. As plataformas podem ser acessadas pulando sobre elas, ou por baixo. Os imigos são spawnados nas laterais da plataforma inferior.
