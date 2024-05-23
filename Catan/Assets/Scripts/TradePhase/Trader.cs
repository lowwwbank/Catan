using Catan.Players;
using Catan.ResourcePhase;
using System.Linq;
using UnityEngine;

namespace Catan.TradePhase
{

    public static class Trader
    {
    
        public static void Trade(Player p1, Player p2, Resource[] p1Offer, Resource[] p2Offer)
        {
            if (p2 == null)
            {
                Trade(p1, p1Offer, p2Offer);
            }

            foreach (Resource r in p1Offer)
            {
                int amount = r.amount;
                p1.resources.Where(rs => rs.type == r.type).First().amount -= amount;
                p2.resources.Where(rs => rs.type == r.type).First().amount += amount;
            }
            foreach (Resource r in p2Offer)
            {
                int amount = r.amount;
                p2.resources.Where(rs => rs.type == r.type).First().amount -= amount;
                p1.resources.Where(rs => rs.type == r.type).First().amount += amount;
            }
        }

        public static void Trade(Player p, Resource[] offer, Resource[] recieve)
        {
            foreach (Resource r in offer)
            {
                int amount = r.amount;
                p.resources.Where(rs => rs.type == r.type).First().amount -= amount;
            }
            foreach (Resource r in recieve)
            {
                int amount = r.amount;
                p.resources.Where(rs => rs.type == r.type).First().amount += amount;
            }
        }

        public static void Discard(Player p, Resource[] offer)
        {
            foreach (Resource r in offer)
            {
                p.resources.Where(rs => rs.type == r.type).First().amount -= r.amount;
            }
        }

        public static void Request(Player p1, Player p2, Resource[] p1Offer, Resource[] p2Offer)
        {
           
                UI.TradePhase tf = GameObject.Find("UI").transform.GetChild(7).GetComponent<UI.TradePhase>();
                tf.ShowOffer(p1, p2, p1Offer, p2Offer);
            
        }
    }
}