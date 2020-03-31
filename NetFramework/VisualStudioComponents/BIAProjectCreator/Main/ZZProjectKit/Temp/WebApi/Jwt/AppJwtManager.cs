namespace $safeprojectname$.Jwt
{
    public class AppJwtManager : BiaJwtManager
    {
        public override string Secret => "0BA9AE2BDC6FBA1056D86EB2ACB45D53072F786FBD94C658240FAE6198F5DC64DE018FE517E68410B9D41B9BE106532345D21090549B0BC396437DCEF3AADAAC";

        public override int Expires => 600;
    }
}