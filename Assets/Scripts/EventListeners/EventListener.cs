using UnityEngine;
using System.Collections;

public enum EventMessage {
	Activate,
	Deactivate,
	ToggleActivate,
	Register,
	Unregister,
	OnLaserHit,
    ToggleUpDown,
    ToggleColor
}

public enum StaticEventMethods
{
    None,
    HideAndDisableObject,
    MakeVisibleAndActive,
    ToggleHideDisableAndActiveVisibleObject,
    TurnOffProjectileAndMedusa,
    TurnOnProjectileAndMedusa,
    ToggleTurnOffProjectileAndMedusa,
    PortalActivateAndMakeTileActive,
    PortalDeactivate,
    PortalToggleActivate
}


public class EventListener : MonoBehaviour
{

    private GridManager gridManager;

	public void ReceiveEvent(EventMessage eventMessage) {
		switch (eventMessage) {
		case EventMessage.Activate :
			// Name of the method
			SendMessage("SetActive", true, SendMessageOptions.DontRequireReceiver);
			break;
		case EventMessage.Deactivate :
			SendMessage("SetActive", false, SendMessageOptions.DontRequireReceiver);
			break;
		case EventMessage.ToggleActivate :
			MonoBehaviour[] mb = GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour b in mb) {
				if (b is IActivatable) {
					IActivatable a = (b as IActivatable);
					a.SetActive(!a.IsActive());
				}
			}
			break;
		case EventMessage.Register :
			SendMessage("Register");
			break;
		case EventMessage.Unregister :
			SendMessage("Unregister");
			break;
		case EventMessage.OnLaserHit :
			SendMessage("OnLaserHit", null, SendMessageOptions.DontRequireReceiver);
			break;
        case EventMessage.ToggleUpDown:
            SendMessage("ToggleUpDown");
            break;
        case EventMessage.ToggleColor:
            SendMessage("ToggleColor");
            break;
		}
	}




    public void ReceiveEventMethod(StaticEventMethods eventMessage, GridManager gridmanager)
    {
        gridManager = gridmanager;

        switch (eventMessage)
        {
            case StaticEventMethods.None:
                break;

            case StaticEventMethods.HideAndDisableObject:
                HideAndDisableObject();
                break;

            case StaticEventMethods.ToggleHideDisableAndActiveVisibleObject:
                ToggleHideAndDisableObject();
                break;

            case StaticEventMethods.TurnOffProjectileAndMedusa:
                TurnOffProjectileAndMedusa();
                break;

            case StaticEventMethods.ToggleTurnOffProjectileAndMedusa:
                ToggleTurnOffProjectileAndMedusa();
                break;

            case StaticEventMethods.MakeVisibleAndActive:
                MakeVisibleAndActive();
                break;

            case StaticEventMethods.TurnOnProjectileAndMedusa:
                TurnOnProjectileAndMedusa();
                break;

            case StaticEventMethods.PortalActivateAndMakeTileActive:
                PortalActivateAndMakeTileActive();
                break;

            case StaticEventMethods.PortalDeactivate:
                PortalDeactivate();
                break;

            case StaticEventMethods.PortalToggleActivate:
                PortalToggleActivate();
                break;

        }
    }


    //Kan finnas smartare lösning än att ha generella metoder här... men de ska vara något som kan nyttjas av alla med en eventlistener..

    private void ToggleHideAndDisableObject()
    {
        BaseUnit bUnit= gameObject.GetComponent<BaseUnit>();

        BaseTile bTile = gameObject.GetComponent<BaseTile>();
       
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);

            //Units logic
            if (bUnit != null)
            {
                bUnit.EventCallOnDeactivated();
            }

            //Tile logic
            if (bTile != null)
            {
                GridManager gridManager = bTile.EventGetGridManager();
                gridManager.RemoveTile(bTile);
            }
        }
        else
        {
            gameObject.SetActive(true);

            //Units logic
            if (bUnit != null)
            {
                if (bUnit.OccupiedTile == null)
                {
                    bUnit.OccupiedTile = gridManager.GetTile(bUnit.transform.position);
                }
                bUnit.EventCallOnActivated();       
            }

            //Tile logic
            if (bTile != null)
            {
                GridManager gridManager = bTile.EventGetGridManager();
                gridManager.AddTile(bTile);
            }
        }     
    }

    private void HideAndDisableObject()
    {
        gameObject.SetActive(false);

        BaseUnit bUnit = gameObject.GetComponent<BaseUnit>();
        BaseTile bTile = gameObject.GetComponent<BaseTile>();

        //Units logic
        if (bUnit != null)
        {
            bUnit.EventCallOnDeactivated();
        }

        //Tile logic
        if (bTile != null)
        {
            GridManager gridManager = bTile.EventGetGridManager();
            gridManager.RemoveTile(bTile);
        }
    }

    private void TurnOffProjectileAndMedusa()
    {
        //Medusa logic
        MedusaUnit medusaUnit;
        medusaUnit = gameObject.GetComponent<MedusaUnit>();

        if (medusaUnit != null)
        {
            medusaUnit.SetActive(false);
        }


        //Projectile logic

        ProjectileShooterUnit projectileUnit;
        projectileUnit = gameObject.GetComponent<ProjectileShooterUnit>();

        if (projectileUnit != null)
        {
            projectileUnit.SetActive(false);
        }

    }

    private void TurnOnProjectileAndMedusa()
    {
        //Medusa logic
        MedusaUnit medusaUnit;
        medusaUnit = gameObject.GetComponent<MedusaUnit>();

        if (medusaUnit != null)
        {
            medusaUnit.SetActive(true);
        }


        //Projectile logic

        ProjectileShooterUnit projectileUnit;
        projectileUnit = gameObject.GetComponent<ProjectileShooterUnit>();

        if (projectileUnit != null)
        {
            projectileUnit.SetActive(true);
        }

    }

    private void ToggleTurnOffProjectileAndMedusa()
    {
        //Medusa logic
        MedusaUnit medusaUnit;
        medusaUnit = gameObject.GetComponent<MedusaUnit>();

        if (medusaUnit != null)
        {
            medusaUnit.SetActive(!medusaUnit.IsActive());
        }


        //Projectile logic

        ProjectileShooterUnit projectileUnit;
        projectileUnit = gameObject.GetComponent<ProjectileShooterUnit>();

        if (projectileUnit != null)
        {
            projectileUnit.SetActive(!projectileUnit.IsActive());
        }

    }

    private void MakeVisibleAndActive()
    {
        gameObject.SetActive(true);

        BaseUnit bUnit = gameObject.GetComponent<BaseUnit>();
        BaseTile bTile = gameObject.GetComponent<BaseTile>();

        //Units logic
        if (bUnit != null)
        {
            bUnit.EventCallOnActivated();
        }

        //Tile logic
        if (bTile != null)
        {
            GridManager gridManager = bTile.EventGetGridManager();
            gridManager.AddTile(bTile);
        }
    }

    private void PortalActivateAndMakeTileActive()
    {
        //Activates the Tile if its inactive
        MakeVisibleAndActive();

        FloorTile floorTile;
        floorTile = gameObject.GetComponent<FloorTile>();

        if (floorTile != null)
        {
            floorTile.IsPortalTile = true;
            floorTile.OnEnable();
        }
    }

    private void PortalDeactivate()
    {
        FloorTile floorTile;
        floorTile = gameObject.GetComponent<FloorTile>();

        if (floorTile != null)
        {
            floorTile.OnDisable();
            floorTile.IsPortalTile = false;
        }
    }

    private void PortalToggleActivate()
    {
        FloorTile floorTile;
        floorTile = gameObject.GetComponent<FloorTile>();

        if (floorTile != null)
        {
            if (floorTile.IsPortalTile)
            {
                floorTile.OnDisable();
                floorTile.IsPortalTile = false;
            }
            else
            {
                floorTile.IsPortalTile = true;
                floorTile.OnEnable();
            }
            
        }
    }
}
