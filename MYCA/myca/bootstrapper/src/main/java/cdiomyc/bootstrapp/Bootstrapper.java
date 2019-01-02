package cdiomyc.bootstrapp;

import cdiomyc.support.actions.Action;

/**
 * Represents MYCA boostrapper
 * @author freitas
 */
public final class Bootstrapper implements Action{
    
    /**
     * Represents MYCA inner boostrappers
     */
    private static final Action[] INNER_BOOTSTRAPPERS ={};
    
    /**
     * Executes MYCA bootstrapper
     * @param args Array with boostrapper arguments
     */
    public static void main(String[] args){
        new Bootstrapper().execute();
    }

    /**
     * Executes the current boostrapper
     */
    @Override
    public void execute() {
        for(int i=0;i<INNER_BOOTSTRAPPERS.length;i++)
            INNER_BOOTSTRAPPERS[i].execute();
    }
}
