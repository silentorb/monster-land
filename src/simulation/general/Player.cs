global using PlayerId = System.Int32;
using monsterland.client.input;
using monsterland.simulation.characters;

namespace monsterland.simulation.general; 

public class Player {
  public PlayerId id = 0;
  public PlayerController controller;
  public CharacterDefinition characterDefinition;

  public Player(PlayerId id) {
    this.id = id;
  }
}
