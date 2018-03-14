using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab1ThreshHold.Infrastructure
{
    public static class Container<TType> where TType:FrameworkElement
    {
        static List<TType> Array=new List<TType>();

        public static void AddToContainer(TType type)
        {
            Array.Add(type);
        }

        public static TTargetType FindElement<TTargetType>(Type baseType, Func<TTargetType, bool> predicate) where TTargetType:FrameworkElement{
            foreach (TType type in Array)
            {
                if (type.GetType().Name==baseType.Name)
                {
                  return type.FindChild<TTargetType>(predicate);
                }
            }
            return null;
        }

        public static TType GetElement(Func<TType, bool> predicate)
        {
            foreach (var type in Array)
            {
                if (predicate(type))
                {
                    return type;
                }
            }
            return null;
        }


    }
}
