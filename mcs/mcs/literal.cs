//
// literal.cs: Literal representation for the IL tree.
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//   Marek Safar (marek.safar@seznam.cz)
//
// Copyright 2001 Ximian, Inc.
//
//
// Notice that during parsing we create objects of type Literal, but the
// types are not loaded (thats why the Resolve method has to assign the
// type at that point).
//
// Literals differ from the constants in that we know we encountered them
// as a literal in the source code (and some extra rules apply there) and
// they have to be resolved (since during parsing we have not loaded the
// types yet) while constants are created only after types have been loaded
// and are fully resolved when born.
//

#if STATIC
using IKVM.Reflection.Emit;
#else
using System.Reflection.Emit;
#endif

namespace Mono.CSharp {

	//
	// The null literal
	//
	// Note: C# specification null-literal is NullLiteral of NullType type
	//
	public class NullLiteral : NullConstant
	{
		//
		// Default type of null is an object
		//
		public NullLiteral (Location loc)
			: base (InternalType.NullLiteral, loc)
		{
		}

		public override void Error_ValueCannotBeConverted (ResolveContext ec, Location loc, TypeSpec t, bool expl)
		{
			if (t.IsGenericParameter) {
				ec.Report.Error(403, loc,
					"Cannot convert null to the type parameter `{0}' because it could be a value " +
					"type. Consider using `default ({0})' instead", t.Name);
				return;
			}

			if (TypeManager.IsValueType (t)) {
				ec.Report.Error(37, loc, "Cannot convert null to `{0}' because it is a value type",
					TypeManager.CSharpName(t));
				return;
			}

			base.Error_ValueCannotBeConverted (ec, loc, t, expl);
		}

		public override string GetValueAsLiteral ()
		{
			return "null";
		}

		public override bool IsLiteral {
			get { return true; }
		}

		public override System.Linq.Expressions.Expression MakeExpression (BuilderContext ctx)
		{
			return System.Linq.Expressions.Expression.Constant (null);
		}
	}

	public class BoolLiteral : BoolConstant {
		public BoolLiteral (BuildinTypes types, bool val, Location loc)
			: base (types, val, loc)
		{
		}

		public override bool IsLiteral {
			get { return true; }
		}
	}

	public class CharLiteral : CharConstant {
		public CharLiteral (BuildinTypes types, char c, Location loc)
			: base (types, c, loc)
		{
		}

		public override bool IsLiteral {
			get { return true; }
		}
	}

	public class IntLiteral : IntConstant {
		public IntLiteral (BuildinTypes types, int l, Location loc)
			: base (types, l, loc)
		{
		}

		public override Constant ConvertImplicitly (TypeSpec type)
		{
			//
			// The 0 literal can be converted to an enum value
			//
			if (Value == 0 && TypeManager.IsEnumType (type)) {
				Constant c = ConvertImplicitly (EnumSpec.GetUnderlyingType (type));
				if (c == null)
					return null;

				return new EnumConstant (c, type);
			}

			return base.ConvertImplicitly (type);
		}

		public override bool IsLiteral {
			get { return true; }
		}
	}

	public class UIntLiteral : UIntConstant {
		public UIntLiteral (BuildinTypes types, uint l, Location loc)
			: base (types, l, loc)
		{
		}

		public override bool IsLiteral {
			get { return true; }
		}
	}
	
	public class LongLiteral : LongConstant {
		public LongLiteral (BuildinTypes types, long l, Location loc)
			: base (types, l, loc)
		{
		}

		public override bool IsLiteral {
			get { return true; }
		}
	}

	public class ULongLiteral : ULongConstant {
		public ULongLiteral (BuildinTypes types, ulong l, Location loc)
			: base (types, l, loc)
		{
		}

		public override bool IsLiteral {
			get { return true; }
		}
	}
	
	public class FloatLiteral : FloatConstant {

		public FloatLiteral (BuildinTypes types, float f, Location loc)
			: base (types, f, loc)
		{
		}

		public override bool IsLiteral {
			get { return true; }
		}

	}

	public class DoubleLiteral : DoubleConstant {
		public DoubleLiteral (BuildinTypes types, double d, Location loc)
			: base (types, d, loc)
		{
		}

		public override void Error_ValueCannotBeConverted (ResolveContext ec, Location loc, TypeSpec target, bool expl)
		{
			if (target.BuildinType == BuildinTypeSpec.Type.Float) {
				Error_664 (ec, loc, "float", "f");
				return;
			}

			if (target.BuildinType == BuildinTypeSpec.Type.Decimal) {
				Error_664 (ec, loc, "decimal", "m");
				return;
			}

			base.Error_ValueCannotBeConverted (ec, loc, target, expl);
		}

		static void Error_664 (ResolveContext ec, Location loc, string type, string suffix)
		{
			ec.Report.Error (664, loc,
				"Literal of type double cannot be implicitly converted to type `{0}'. Add suffix `{1}' to create a literal of this type",
				type, suffix);
		}

		public override bool IsLiteral {
			get { return true; }
		}

	}

	public class DecimalLiteral : DecimalConstant {
		public DecimalLiteral (BuildinTypes types, decimal d, Location loc)
			: base (types, d, loc)
		{
		}

		public override bool IsLiteral {
			get { return true; }
		}
	}

	public class StringLiteral : StringConstant {
		public StringLiteral (BuildinTypes types, string s, Location loc)
			: base (types, s, loc)
		{
		}

		public override bool IsLiteral {
			get { return true; }
		}

	}
}
