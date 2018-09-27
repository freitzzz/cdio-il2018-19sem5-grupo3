using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.domain {
    public abstract class Authentication {
        /// <summary>
        /// Authentication token
        /// </summary>
        private string token;

        public Authentication() {
            this.token = generateToken();
        }

        public abstract bool authenticate();
        protected abstract string generateToken();
        override
        public bool Equals(Object obj) {
            if (obj == null || !this.GetType().Equals(obj.GetType())) {
                return false;
            }
            if (this == obj) {
                return true;
            }
            Authentication auth = (Authentication)obj;
            if (this.token.Equals(auth.token)) {
                return true;
            } else {
                return false;
            }
        }
        override
        public int GetHashCode() {
            return token.GetHashCode();
        }
    }
}
