using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using TS.Player;

namespace MoreMountains.TopDownEngine
{
	/// <summary>
	/// An event typically fired when picking an item, letting listeners know what item has been picked
	/// </summary>
	public struct PickableItemEvent
	{
		public GameObject Picker;
		public PickableItem PickedItem;

		/// <summary>
		/// Initializes a new instance of the <see cref="MoreMountains.TopDownEngine.PickableItemEvent"/> struct.
		/// </summary>
		/// <param name="pickedItem">Picked item.</param>
		public PickableItemEvent(PickableItem pickedItem, GameObject picker) 
		{
			Picker = picker;
			PickedItem = pickedItem;
		}
		static PickableItemEvent e;
		public static void Trigger(PickableItem pickedItem, GameObject picker)
		{
			e.Picker = picker;
			e.PickedItem = pickedItem;
			MMEventManager.TriggerEvent(e);
		}
	}

	public class PickableItem : MonoBehaviour
	{
		[Header("Pickable Item")]                        
		/// the duration (in seconds) after which to disable the object, instant if 0
		public float DisableDelay = 0f;
		protected Collider2D _collider2D;
		protected GameObject _collidingObject;
		protected bool _pickable = false;
		protected WaitForSeconds _disableDelay;
		protected PlayerControl _character = null;

		protected virtual void Start()
		{
			_disableDelay = new WaitForSeconds(DisableDelay);
			_collider2D = gameObject.GetComponent<Collider2D>();
		}

		/// <summary>
		/// Triggered when something collides with the coin
		/// </summary>
		/// <param name="collider">Other.</param>
		public virtual void OnTriggerEnter2D (Collider2D collider) 
		{
			_collidingObject = collider.gameObject;
			PickItem (collider.gameObject);
		}

		/// <summary>
		/// Check if the item is pickable and if yes, proceeds with triggering the effects and disabling the object
		/// </summary>
		public virtual void PickItem(GameObject picker)
		{
			if (CheckIfPickable ())
			{
				Effects ();
				PickableItemEvent.Trigger(this, picker);
				Pick (picker);

				if (DisableDelay == 0f)
				{
					this.gameObject.SetActive(false);
				}
				else
				{
					StartCoroutine(DisablePickerCoroutine());
				}		
			} 
		}

		protected virtual IEnumerator DisablePickerCoroutine()
		{
			yield return _disableDelay;
			this.gameObject.SetActive(false);
		}

		/// <summary>
		/// Checks if the object is pickable.
		/// </summary>
		/// <returns><c>true</c>, if if pickable was checked, <c>false</c> otherwise.</returns>
		protected virtual bool CheckIfPickable()
		{
			_character = _collidingObject.GetComponent<PlayerControl>();

			if (_character == null)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Triggers the various pick effects
		/// </summary>
		protected virtual void Effects()
		{
			
		}

		/// <summary>
		/// Override this to describe what happens when the object gets picked
		/// </summary>
		protected virtual void Pick(GameObject picker)
		{
			
		}
	}
}