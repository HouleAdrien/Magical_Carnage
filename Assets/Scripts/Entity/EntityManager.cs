using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
   private static EntityManager instance;

    List<Entity> entities = new List<Entity>(); 

   public static EntityManager Instance {  
        get { 
            if (instance == null)
            {
                instance = new EntityManager();
            }
            return instance; 
        }
   }

    GameObject player; public GameObject Player { get { return player; } }

    public void RegisterPlayer(GameObject p) { player = p; }

    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        entities.Remove(entity);
        player.GetComponent<Player>().AddKill();
    }

    public List<Entity> GetEntities()
    {
        return entities;
    }
  
}
