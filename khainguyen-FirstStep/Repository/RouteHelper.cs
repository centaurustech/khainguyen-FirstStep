using System.Web;
using System.Web.Routing;


namespace SGI.Repository
{

     
        public class GioiThieuRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                if ((string)values["controller"] == "GioiThieu" && (string)values["action"] == "Index")
                    return true;

                return false;
            }            
    }
        public class DuAnRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                if ((string)values["controller"] == "DuAn" && (string)values["action"] == "Index")
                    return true;

                return false;
            }
        }
        public class ConNguoiRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                if ((string)values["controller"] == "ConNguoi" && (string)values["action"] == "Index")
                    return true;

                return false;
            }
        }
        public class LienHeRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                if ((string)values["controller"] == "LienHe" && (string)values["action"] == "Index")
                    return true;

                return false;
            }
        }
        public class IndexListRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                if ((string)values["controller"] == "Product" && (string)values["action"] == "IndexList")
                    return true;

                return false;
            }
        }
        public class IndexspRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                if ((string)values["controller"] == "Product" && (string)values["action"] == "ListProduct")
                    return true;

                return false;
            }
        }
        public class otherspRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                if ((string)values["controller"] == "Product" && (string)values["action"] == "OrtherListProduct")
                    return true;

                return false;
            }
        }



}