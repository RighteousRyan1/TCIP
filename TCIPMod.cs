using Terraria.ModLoader;

namespace TCIPMod
{
	public class TCIPMod : Mod
	{
        IDetour d; 

        public override void Load()
        {
            MonoModHooks.RequestNativeAccess();

            var test = typeof(Activator).GetMethod("CreateInstance", new Type[] { typeof(Type) } );
            var test2 = typeof(AmazingMod).GetMethod("MagicPatch2", BindingFlags.Instance | BindingFlags.NonPublic);

            d = new Hook(test, test2, this);

            d.Apply();
        }

        private object MagicPatch2(Func<Type, object> orig, Type type)
        {
            return new Trollface();
        }
	}
	    public class Trollface : Object
    {

    }
}
