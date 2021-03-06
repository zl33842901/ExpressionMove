﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace xLiAd.ExpressionMove
{
    internal class TrimExpression : ExpressionVisitor
    {
        private bool IsDeep = false;
        internal static Expression Trim(Expression expression)
        {
            return new TrimExpression().Visit(expression);
        }

        private Expression Sub(Expression expression)
        {
            var type = expression.Type;
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    if (TypeHelper.GetNonNullableType(expression.Type) == TypeHelper.GetNonNullableType(type))
                        return Expression.Constant(((ConstantExpression)expression).Value, type);
                    break;

                case ExpressionType.MemberAccess:
                    var mExpression = expression as MemberExpression;
                    var root = mExpression.GetRootMember();
                    if (root != null)
                    {
                        var value = mExpression.MemberToValue(root);
                        return Expression.Constant(value, type);
                    }
                    else
                    {
                        if (IsDeep)
                            return expression;

                        IsDeep = true;
                        return Expression.Equal(expression, Expression.Constant(true));
                    }

                case ExpressionType.Convert:
                    var u = (UnaryExpression)expression;
                    ////////////////当发生非空类型和空类型的转换时
                    if (TypeHelper.GetNonNullableType(u.Operand.Type) == TypeHelper.GetNonNullableType(type))
                    {
                        expression = u.Operand;
                        if (expression.NodeType == ExpressionType.MemberAccess)
                        {
                            var s = Sub(expression);
                            if (s.NodeType == ExpressionType.Constant)
                            {
                                var ss = s as ConstantExpression;
                                if (u.Type != ss.Type)
                                {
                                    return Expression.Constant(ss.Value, u.Type);
                                }
                                else
                                {
                                    return ss;
                                }
                            }
                            else
                                return u;
                        }
                        else if (expression.NodeType == ExpressionType.Constant)
                        {

                            return Expression.Constant(((ConstantExpression)expression).Value, u.Type);
                        }
                        else
                            return expression;
                    }

                    if (u.Operand.Type.IsEnum && u.Operand.NodeType == ExpressionType.MemberAccess)
                    {
                        object oo = (u.Operand as MemberExpression).MemberToValue();
                        if (oo != null)
                        {
                            var value = Convert.ChangeType(oo, TypeHelper.GetNonNullableType(type));
                            return Expression.Constant(value, type);
                        }
                    }
                    break;

                //case ExpressionType.Not:
                //    var n = (UnaryExpression)expression;
                //    return Expression.Equal(n.Operand, Expression.Constant(false));
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    var b = (BinaryExpression)expression;
                    IsDeep = true;
                    if (b.Left.NodeType != b.Right.NodeType)
                    {
                        if (b.Left.NodeType == ExpressionType.MemberAccess && b.Left.Type.Name == "Boolean")
                        {
                            if (expression.NodeType == ExpressionType.AndAlso)
                                return Expression.AndAlso(Expression.Equal(b.Left, Expression.Constant(true)), b.Right);
                            if (expression.NodeType == ExpressionType.OrElse)
                                return Expression.OrElse(Expression.Equal(b.Left, Expression.Constant(true)), b.Right);
                        }
                        if (b.Right.NodeType == ExpressionType.MemberAccess && b.Right.Type.Name == "Boolean")
                        {
                            if (expression.NodeType == ExpressionType.AndAlso)
                                return Expression.AndAlso(b.Left, Expression.Equal(b.Right, Expression.Constant(true)));
                            if (expression.NodeType == ExpressionType.OrElse)
                                return Expression.OrElse(b.Left, Expression.Equal(b.Right, Expression.Constant(true)));
                        }
                        if (b.Left.NodeType == ExpressionType.Constant)
                            return b.Right;
                        if (b.Right.NodeType == ExpressionType.Constant)
                            return b.Left;
                    }
                    break;
                default:
                    IsDeep = true;
                    return expression;
            }

            return expression;
        }

        public override Expression Visit(Expression exp)
        {
            if (exp == null)
                return null;

            exp = Sub(exp);
            try
            {
                var result = base.Visit(exp);
                return result;
            }
            catch (Exception e)
            {
                return exp;
            }
        }
    }
}
