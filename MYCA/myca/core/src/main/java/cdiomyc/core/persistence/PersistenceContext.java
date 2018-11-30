/*
 * The MIT License
 *
 * Copyright 2018 freitas.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
package cdiomyc.core.persistence;

import cdiomyc.core.application.Application;

/**
 * Persistence Utility class that represents the factory of persistence repositories 
 * factories for a certain persistence context
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class PersistenceContext {
    /**
     * Constant that represents the message that occurres if the current 
     * persistence context is not defined
     */
    private static final String PERSISTENCE_CONTEXT_NOT_DEFINED_MESSAGE
            ="The persistence context is not defined!";
    /**
     * RepositoryFactory with the current persistence context repository factory
     */
    private static RepositoryFactory contextRepositoryFactory;
    /**
     * Method that returns the repositories of the current persistence context
     * @return RepositoryFactory with the factory that allows the repositories 
     * retrieval of the current persistence context
     */
    public static RepositoryFactory repositories(){
        if(contextRepositoryFactory==null){
            try {
                return (RepositoryFactory)
                        Class.forName(Application.settings().getPersistenceContext()).newInstance();
            } catch (ClassNotFoundException | InstantiationException | IllegalAccessException persistenceContextException) {
                throw new IllegalStateException(PERSISTENCE_CONTEXT_NOT_DEFINED_MESSAGE,persistenceContextException);
            }
        }
        return contextRepositoryFactory;
    }
    /**
     * Hides default constructor
     */
    private PersistenceContext(){}
}
