package cdiomyc.core.persistence;

import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.Auth;
import cdiomyc.support.domain.ddd.Repository;

/**
 * Represents the repository functionalities for users
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public interface UserRepository extends Repository<User,Long,Auth>{}
